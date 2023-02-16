using Microsoft.EntityFrameworkCore;
using System;

namespace Seom.Application.Model
{
    [Index(nameof(Name), IsUnique = true)]
    public class Project
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Project() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Project(string name, DateTime start, string? repo = null, string? customer = null, DateTime? finished = null)
        {
            Name = name;
            Start = start;
            Repo = repo;
            Customer = customer;
            Finished = finished;
        }
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public string? Repo { get; set; }
        public string? Customer { get; set; }
        public DateTime? Finished { get; set; }
    }
}
