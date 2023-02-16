using Bogus;
using Microsoft.EntityFrameworkCore;
using Seom.Application.Model;
using System;
using System.Linq;

namespace Seom.Application.Infrastructure
{
    public class SeomContext : DbContext
    {
        public DbSet<Milestone> Milestones => Set<Milestone>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Task> Tasks => Set<Task>();
        public DbSet<WorkItem> WorkItems => Set<WorkItem>();

        public SeomContext(DbContextOptions<SeomContext> opt) : base(opt) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Generic config for all entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // ON DELETE RESTRICT instead of ON DELETE CASCADE
                foreach (var key in entityType.GetForeignKeys())
                    key.DeleteBehavior = DeleteBehavior.Restrict;

                foreach (var prop in entityType.GetDeclaredProperties())
                {
                    // Define Guid as alternate key. The database can create a guid fou you.
                    if (prop.Name == "Guid")
                    {
                        modelBuilder.Entity(entityType.ClrType).HasAlternateKey("Guid");
                        prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                    }
                    // Default MaxLength of string Properties is 255.
                    if (prop.ClrType == typeof(string) && prop.GetMaxLength() is null) prop.SetMaxLength(255);
                    // Seconds with 3 fractional digits.
                    if (prop.ClrType == typeof(DateTime)) prop.SetPrecision(3);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetPrecision(3);
                }
            }
        }

        /// <summary>
        /// Creates the database. Called once at application startup.
        /// </summary>
        public void CreateDatabase(bool isDevelopment)
        {
            if (isDevelopment) { Database.EnsureDeleted(); }
            // EnsureCreated only creates the model if the database does not exist or it has no
            // tables. Returns true if the schema was created.  Returns false if there are
            // existing tables in the database. This avoids initializing multiple times.
            if (Database.EnsureCreated()) { Initialize(); }
            if (isDevelopment) Seed();
        }

        /// <summary>
        /// Initialize the database with some values (holidays, ...).
        /// Unlike Seed, this method is also called in production.
        /// </summary>
        private void Initialize()
        {

        }

        private void Seed()
        {
            Randomizer.Seed = new Random(1709);
            var baseDate = DateTime.UtcNow.Date;

            var projects = new Faker<Project>("de").CustomInstantiator(f =>
            {
                var name = f.Commerce.Product();
                var start = baseDate.AddDays(f.Random.Int(-2 * 365, -14));
                DateTime? finished = start.AddDays(f.Random.Int(365, 2 * 365));
                if (finished > baseDate) { finished = null; }

                return new Project(
                    name: name, start: start,
                    repo: $"https://github.com/taurus_company/{name.ToLower()}".OrNull(f, 0.5f),
                    customer: f.Company.CompanyName(),
                    finished: finished)
                { Guid = f.Random.Guid() };
            })
            .Generate(10)
            .GroupBy(p => p.Name).Select(g => g.First())
            .ToList();
            Projects.AddRange(projects);
            SaveChanges();

            var milestones = new Faker<Milestone>("de").CustomInstantiator(f =>
            {
                var project = f.Random.ListItem(projects);
                var datePlanned = f.Date.Between(project.Start, project.Finished ?? baseDate.AddDays(90));
                var dateFinished = datePlanned.AddDays(f.Random.Int(-14, 14)).OrNull(f, 0.5f);
                if (dateFinished > baseDate) { dateFinished = null; }
                return new Milestone(
                    project: project, name: f.Lorem.Sentence(5),
                    datePlanned: datePlanned, dateFinished: dateFinished)
                { Guid = f.Random.Guid() };
            })
            .Generate(50)
            .ToList();
            Milestones.AddRange(milestones);
            SaveChanges();

            var tasks = new Faker<Task>("de").CustomInstantiator(f =>
            {
                var milestone = f.Random.ListItem(milestones);
                return new Task(milestone: milestone, text: f.Lorem.Sentence(5))
                {
                    Guid = f.Random.Guid(),
                    Fullfilled = f.Random.Bool(0.4f)
                };
            })
            .Generate(200)
            .ToList();
            Tasks.AddRange(tasks);
            SaveChanges();

            var workItems = new Faker<WorkItem>("de").CustomInstantiator(f =>
            {
                var project = f.Random.ListItem(projects);
                var from = f.Date.Between(project.Start, project.Finished ?? baseDate).Date + TimeSpan.FromMinutes(f.Random.Int(8 * 60, 14 * 60));
                var to = from + TimeSpan.FromMinutes(f.Random.Int(1 * 60, 8 * 60));
                return new WorkItem(project: project, name: f.Lorem.Sentence(5), from: from, to: to) { Guid = f.Random.Guid() };
            })
            .Generate(50)
            .ToList();
            WorkItems.AddRange(workItems);
            SaveChanges();
        }
    }
}
