using System;
using System.Security.Cryptography;

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
        public double WorkingHoursInWeek(DateTime startOfWeek)
        {
            if (To < startOfWeek) { return 0; }
            var startOfNextWeek = startOfWeek.AddDays(7);
            if (From >= startOfNextWeek) { return 0; }
            var to = To > startOfNextWeek ? startOfNextWeek : To;
            return (to - From).TotalHours;
        }
        public static DateTime CalcStartOfWeek(DateTime dateTime) => dateTime.AddDays(-(((int)dateTime.DayOfWeek + 6) % 7)).Date;
    }
}
