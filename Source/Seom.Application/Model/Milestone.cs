using System;
using System.Collections.Generic;

namespace Seom.Application.Model
{
    public class Milestone
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Milestone() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Milestone(Project project, string name, DateTime datePlanned, DateTime? dateFinished = null)
        {
            Project = project;
            Name = name;
            DatePlanned = datePlanned;
            DateFinished = dateFinished;
        }
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public DateTime DatePlanned { get; set; }
        public DateTime? DateFinished { get; set; }
        public List<Task> Tasks { get; } = new();
    }
}
