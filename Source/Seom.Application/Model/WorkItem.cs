using System;

namespace Seom.Application.Model
{
    public class WorkItem
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected WorkItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public WorkItem(Project project, string name, DateTime from, DateTime to)
        {
            Project = project;
            Name = name;
            From = from;
            To = to;
        }
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
