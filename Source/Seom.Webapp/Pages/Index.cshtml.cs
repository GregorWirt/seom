using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Seom.Application.Dtos;
using Seom.Application.Infrastructure;
using Seom.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seom.Webapp.Pages;

public class IndexModel : PageModel
{
    private readonly SeomContext _db;
    public record WorkloadDto(DateTime FirstDayOfWeek, double WorkingHours);
    public record MilestoneDto(string Name, Guid ProjectGuid, string ProjectName, DateTime DatePlanned, int OpenTasks);
    public record CurrentProjectDto(string Name, DateTime Start, double HoursTotal);

    public SelectList Projects { get; set; } = default!;
    public List<CurrentProjectDto> CurrentProjects { get; set; } = new();
    public List<MilestoneDto> CurrentMilestones { get; set; } = new();
    public IndexModel(SeomContext db)
    {
        _db = db;
    }

    public void OnGet()
    {
        CurrentProjects = _db.Projects
            .Include(p => p.WorkItems)
            .Where(p => p.Finished == null)
            .OrderBy(p => p.Start)
            .ToList()
            .Select(p => new CurrentProjectDto(
                p.Name,
                p.Start,
                p.WorkItems.Sum(w => (w.To - w.From).TotalHours)))
            .ToList();
        CurrentMilestones = _db.Milestones
            .Where(m => m.DateFinished == null && m.DatePlanned < DateTime.Now.AddDays(14))
            .OrderBy(m => m.DatePlanned)
            .Select(m => new MilestoneDto(
                m.Name,
                m.Project.Guid,
                m.Project.Name,
                m.DatePlanned,
                m.Tasks.Count(t => !t.Fullfilled)))
            .ToList();
    }

    public IActionResult OnGetWorkload([FromQuery] Guid projectGuid)
    {
        var epoch = new DateTime(1970, 1, 1);
        var workItems = (projectGuid != default ? _db.WorkItems.Where(w => w.Project.Guid == projectGuid) : _db.WorkItems).ToList();
        if (!workItems.Any()) { return new JsonResult(Array.Empty<decimal[]>()); }

        var workloads = new List<WorkloadDto>();
        var end = workItems.Max(w => w.To);
        for (var date = WorkItem.CalcStartOfWeek(workItems.Min(w => w.From)); date <= end; date = date.AddDays(7))
        {
            var workingHours = workItems.Sum(w => w.WorkingHoursInWeek(date));
            workloads.Add(new WorkloadDto(FirstDayOfWeek: date, WorkingHours: workingHours));
        }
        return new JsonResult(workloads.Select(w => new decimal[] { (long)(w.FirstDayOfWeek - epoch).TotalMilliseconds, (decimal)w.WorkingHours }));
    }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        Projects = new SelectList(_db.Projects.OrderBy(p => p.Name), nameof(Project.Guid), nameof(Project.Name), "");
    }
}
