using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Seom.Application.Infrastructure;
using Seom.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seom.Webapp.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly SeomContext _db;

        public Dictionary<Guid, Milestone> Milestones { get; set; } = new();
        public SelectList Projects { get; set; } = default!;
        [FromRoute]
        public Guid ProjectGuid { get; set; }
        [BindProperty]
        public Dictionary<Guid, bool> TaskFullfilled { get; set; } = new();
        [BindProperty]
        public Dictionary<Guid, string> NewTasks { get; set; } = new();
        public IndexModel(SeomContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            TaskFullfilled = Milestones.Values.SelectMany(m => m.Tasks).ToDictionary(t => t.Guid, t => t.Fullfilled);
            NewTasks = Milestones.Keys.ToDictionary(m => m, m => string.Empty);
        }

        public IActionResult OnPost()
        {
            var tasks = Milestones.Values.SelectMany(m => m.Tasks).ToDictionary(t => t.Guid, t => t);
            foreach (var fullfilled in TaskFullfilled)
            {
                if (!tasks.TryGetValue(fullfilled.Key, out var task)) { continue; }
                task.Fullfilled = fullfilled.Value;
            }
            _db.SaveChanges();
            foreach (var newTask in NewTasks.Where(n => !string.IsNullOrEmpty(n.Value)))
            {
                if (!Milestones.TryGetValue(newTask.Key, out var milestone)) { continue; }
                var task = new Task(milestone: milestone, text: newTask.Value);
                _db.Tasks.Add(task);
            }
            _db.SaveChanges();
            return RedirectToPage();
        }
        /// <summary>
        /// Before model binding
        /// </summary>
        public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {

        }


        /// <summary>
        /// After model binding
        /// </summary>
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            Projects = new SelectList(_db.Projects.OrderBy(p => p.Name), nameof(Project.Guid), nameof(Project.Name), "");
            Milestones = _db.Milestones
                .Include(m => m.Tasks)
                .Where(m => m.Project.Guid == ProjectGuid && m.DateFinished == null)
                .ToDictionary(m => m.Guid, m => m);
        }
    }
}
