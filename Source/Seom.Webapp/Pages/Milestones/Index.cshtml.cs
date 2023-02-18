using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Seom.Application.Dtos;
using Seom.Application.Infrastructure;
using Seom.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seom.Webapp.Pages.Milestones
{
    public class IndexModel : PageModel
    {
        private readonly SeomContext _db;
        public SelectList Projects { get; set; } = default!;
        public Dictionary<Guid, Milestone> Milestones { get; set; } = new();
        public Dictionary<Guid, MilestoneDto> MilestoneDtos { get; set; } = new();
        public MilestoneDto? NewMilestoneDto { get; set; }

        [FromRoute]
        public Guid ProjectGuid { get; set; }
        public IndexModel(SeomContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostEditMilestones(Dictionary<Guid, MilestoneDto> milestoneDtos)
        {
            MilestoneDtos = milestoneDtos;
            if (!ModelState.IsValid) { return Page(); }
            foreach (var m in milestoneDtos)
            {
                if (!Milestones.TryGetValue(m.Key, out var milestone)) { continue; }
                milestone.Name = m.Value.Name;
                milestone.DatePlanned = m.Value.DatePlanned;
                milestone.DateFinished = m.Value.DateFinished;
            }
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", e.InnerException?.Message ?? e.Message);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostNewMilestone(MilestoneDto newMilestoneDto)
        {
            NewMilestoneDto = newMilestoneDto;
            if (!ModelState.IsValid) { return Page(); }

            var project = _db.Projects.FirstOrDefault(p => p.Guid == ProjectGuid);
            if (project is null) { return RedirectToPage(); }

            var milestone = new Milestone(
                project: project,
                name: newMilestoneDto.Name, datePlanned: newMilestoneDto.DatePlanned, dateFinished: newMilestoneDto.DateFinished);
            _db.Milestones.Add(milestone);
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", e.InnerException?.Message ?? e.Message);
                return Page();
            }
            return RedirectToPage();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            Projects = new SelectList(_db.Projects.OrderBy(p => p.Name), nameof(Project.Guid), nameof(Project.Name), "");
            Milestones = _db.Milestones.Where(m => m.Project.Guid == ProjectGuid)
                .ToDictionary(m => m.Guid, m => m);
            MilestoneDtos = Milestones.Values
                .ToDictionary(m => m.Guid, m => new MilestoneDto(
                    Guid: m.Guid, Name: m.Name, DatePlanned: m.DatePlanned, DateFinished: m.DateFinished));
        }
    }
}
