using System;

namespace Seom.Application.Model
{
    public class Task
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Task() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Task(Milestone milestone, string text)
        {
            Milestone = milestone;
            Text = text;
        }
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Milestone Milestone { get; set; }
        public string Text { get; set; }
        public bool Fullfilled { get; set; }

    }
}
