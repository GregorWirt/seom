using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Seom.Application.Dtos;
using Seom.Application.Infrastructure;
using Seom.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seom.Webapp.Pages.Timesheet
{
    public class IndexModel : PageModel
    {
        private readonly SeomContext _db;

        public IndexModel(SeomContext db)
        {
            _db = db;
        }

        public SelectList Projects { get; set; } = default!;
        public List<WorkItem> WorkItems { get; set; } = new();
        [FromRoute]
        public Guid ProjectGuid { get; set; }
        [BindProperty]
        public WorkItemDto? NewWorkItem { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPostWorkitem()
        {
            if (!ModelState.IsValid) { return Page(); }
            if (NewWorkItem is null) { return RedirectToPage(); }
            var project = _db.Projects.FirstOrDefault(p => p.Guid == ProjectGuid);
            if (project is null) { return RedirectToPage(); }
            var workitem = new WorkItem(project: project, name: NewWorkItem.Name, from: NewWorkItem.From, to: NewWorkItem.To);
            _db.WorkItems.Add(workitem);
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", e.Message);
                return Page();
            }
            return RedirectToPage();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            Projects = new SelectList(_db.Projects.OrderBy(p => p.Name), nameof(Project.Guid), nameof(Project.Name), "");
            WorkItems = _db.WorkItems.Where(w => w.Project.Guid == ProjectGuid)
                .OrderBy(p => p.From)
                .ToList();
        }
    }
}
