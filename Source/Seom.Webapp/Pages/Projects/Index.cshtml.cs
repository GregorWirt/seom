using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Seom.Application.Dtos;
using Seom.Application.Infrastructure;
using Seom.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seom.Webapp.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly SeomContext _db;
        public Dictionary<Guid, Project> Projects { get; set; } = new();
        public Dictionary<Guid, ProjectDto> ProjectDtos { get; set; } = new();
        public ProjectDto? NewProjectDto { get; set; }
        public IndexModel(SeomContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostEditProjects(Dictionary<Guid, ProjectDto> projectDtos)
        {
            ProjectDtos = projectDtos;
            if (!ModelState.IsValid) { return Page(); }
            foreach (var p in ProjectDtos)
            {
                if (!Projects.TryGetValue(p.Key, out var project)) { continue; }
                project.Name = p.Value.Name;
                project.Customer = p.Value.Customer;
                project.Repo = p.Value.Repo;
                project.Start = p.Value.Start;
                project.Finished = p.Value.Finished;
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

        public IActionResult OnPostNewProject(ProjectDto newProjectDto)
        {
            NewProjectDto = newProjectDto;
            if (!ModelState.IsValid) { return Page(); }

            var project = new Project(
                name: newProjectDto.Name, customer: newProjectDto.Customer,
                start: newProjectDto.Start, repo: newProjectDto.Repo, finished: newProjectDto.Finished);
            _db.Projects.Add(project);
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
            Projects = _db.Projects.ToDictionary(p => p.Guid, p => p);
            ProjectDtos = Projects.Values
                .ToDictionary(p => p.Guid, p => new ProjectDto(
                    Guid: p.Guid,
                    Name: p.Name, Customer: p.Customer, Repo: p.Repo,
                    Start: p.Start, Finished: p.Finished));
        }
    }
}
