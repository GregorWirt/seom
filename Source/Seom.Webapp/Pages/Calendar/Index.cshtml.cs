using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Seom.Application.Infrastructure;
using Seom.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seom.Webapp.Pages.Calendar
{
    public class IndexModel : PageModel
    {
        public record CalendarDayDto(
            DateTime DateTime, bool IsWorkingDayMoFr, bool IsPublicHoliday, string? PublicHolidayName,
            List<MilestoneDto> MilestoneDtos);
        public record MilestoneDto(
            string Name, Guid ProjectGuid, string ProjectName, DateTime? DateFinished,
            DateTime DatePlanned, bool IsFinished, bool Delayed, DateTime FinishedOrPlanned);

        private readonly CalendarService _calendar;
        private readonly SeomContext _db;
        private static readonly string[] _monthNames = new string[] { "", "Jänner", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };
        public IndexModel(CalendarService calendar, SeomContext db)
        {
            _calendar = calendar;
            _db = db;
        }
        public List<CalendarDayDto> CalendarDayDtos { get; set; } = new();
        [FromRoute]
        public int Year { get; set; }
        [FromRoute]
        public int Month { get; set; }
        public string MonthName => _monthNames[Month];
        public int PrevMonth => Month == 1 ? 12 : Month - 1;
        public int PrevYear => Month == 1 ? Year - 1 : Year;
        public int NextMonth => Month == 12 ? 1 : Month + 1;
        public int NextYear => Month == 12 ? Year + 1 : Year;

        public void OnGet()
        {
            var calendarDays = _calendar.GetDaysOfMonthFullWeeks(Year, Month);
            var milestones = _db.Milestones
                .Where(m => (m.DateFinished ?? m.DatePlanned).Year == Year && (m.DateFinished ?? m.DatePlanned).Month == Month)
                .Select(m=> new MilestoneDto(
                    m.Name,
                    m.Project.Guid,
                    m.Project.Name,
                    m.DateFinished,
                    m.DatePlanned,
                    m.DateFinished != null,
                    m.Delayed,
                    m.DateFinished ?? m.DatePlanned
                ))
                .ToList();
            CalendarDayDtos = calendarDays.GroupJoin(milestones, c => c.DateTime, m => m.FinishedOrPlanned, (c, milestones) => new CalendarDayDto(
                c.DateTime,
                c.IsWorkingDayMoFr,
                c.IsPublicHoliday,
                c.PublicHolidayName,
                milestones.ToList()
            ))
            .ToList();
        }
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (Year < 2000 || Year > 2100) { Year = DateTime.UtcNow.Year; }
            if (Month < 1 || Month > 12) { Month = DateTime.UtcNow.Month; }
        }
    }

}
