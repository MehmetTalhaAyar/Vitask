using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Vitask.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        private readonly UserManager<AppUser> _userService;

        public ProjectController(IProjectService projectService, UserManager<AppUser> userManager)
        {
            _projectService = projectService;
            _userService = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserAsync(User);
            
            if(user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var values = _projectService.GetAllByUserId(user.Id);

            // admin tüm projeleri görebilecek
            //var values = _projectService.GetAll();


            return View(values);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddProject()
        {
            return View();
        }
		[Authorize]
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

		[Authorize]
		public IActionResult DeleteProject(int id)
        {
            var value = _projectService.GetById(id);
            _projectService.Delete(value);
            return RedirectToAction("Index");
        }
		[Authorize]
		[HttpGet]
        public IActionResult EditProject(int id)
        {
            var value = _projectService.GetById(id);
            return View(value);
        }
		[Authorize]
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