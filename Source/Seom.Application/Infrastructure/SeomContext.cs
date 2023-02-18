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
            var faker = new Faker("de");
            var baseDate = DateTime.UtcNow.Date;



            var projects = new Project[]
            {
                new Project(name: "FluxApp", customer: "Apple", start: baseDate.AddDays(faker.Random.Int(-100, -14)), repo: "https://github.com/taurus/fluxapp")
                    { Guid = faker.Random.Guid() },
                new Project(name: "DataMapper", customer: "Amazon", start: baseDate.AddDays(faker.Random.Int(-100, -14)), repo: "https://github.com/taurus/DataMapper")
                    { Guid = faker.Random.Guid() },
                new Project(name: "CloudQA", customer: "Microsoft", start: baseDate.AddDays(faker.Random.Int(-100, -14)))
                    { Guid = faker.Random.Guid() },
                new Project(name: "InnoTrack", customer: "Google", start: baseDate.AddDays(faker.Random.Int(-100, -14)), repo: "https://github.com/taurus/InnoTrack")
                    { Guid = faker.Random.Guid() },
                new Project(name: "SwiftDrive", customer: "Walmart", start: baseDate.AddDays(faker.Random.Int(-100, -14)))
                    { Guid = faker.Random.Guid() }
            };
            Projects.AddRange(projects);
            SaveChanges();

            var milestoneNames = new string[] { "Establish project goals and objectives", "Create project plan and timeline", "Develop software architecture", "Design user interface", "Write code and build software", "Conduct unit testing", "Perform system and integration testing", "Complete acceptance testing", "Develop user documentation", "Train users", "Deploy software", "Monitor system performance", "Troubleshoot software issues", "Update software", "Enhance existing features", "Develop new features", "Refactor code", "Maintain version control", "Create backup and recovery plan", "Evaluate and document lessons learned" };
            var milestones = new Faker<Milestone>("de").CustomInstantiator(f =>
            {
                var project = f.Random.ListItem(projects);
                var datePlanned = f.Date.Between(project.Start, project.Finished ?? baseDate.AddDays(90)).Date;
                DateTime? dateFinished = datePlanned.AddDays(f.Random.Int(-14, 14));
                if (dateFinished > baseDate) { dateFinished = null; }
                if (dateFinished > baseDate.AddDays(-14)) { dateFinished = f.Random.Bool(0.5f) ? null : dateFinished; }
                return new Milestone(
                    project: project, name: f.Random.ListItem(milestoneNames),
                    datePlanned: datePlanned, dateFinished: dateFinished)
                { Guid = f.Random.Guid() };
            })
            .Generate(25)
            .GroupBy(m => m.Name).Select(g => g.First())
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
            .Generate(100)
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
