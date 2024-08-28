using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Vitask.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        public IActionResult Index()
        {
            var values = _projectService.GetAll();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddProject()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddProject(Project project)
        {
            ProjectValidator validationRules = new ProjectValidator();
            ValidationResult result = validationRules.Validate(project); if (result.IsValid)
            {
                _projectService.Insert(project);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();

        }
        public IActionResult DeleteProject(int id)
        {
            var value = _projectService.GetById(id);
            _projectService.Delete(value);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditProject(int id)
        {
            var value = _projectService.GetById(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult EditProject(Project project)
        {
            ProjectValidator validationRules = new ProjectValidator();
            ValidationResult result = validationRules.Validate(project);
            if (result.IsValid)
            {
                _projectService.Update(project);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

            }
            return View();
        }
    }
}