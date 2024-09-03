using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vitask.Models;
using Task = Entities.Concrete.Task;
namespace Vitask.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        private readonly UserManager<AppUser> _userService;

		private readonly IAppUserService _appUserService;

        private readonly IProjectUserService _projectUserService;

        private readonly ITaskService _taskService;

        private readonly ITagService _tagService;

		public ProjectController(IProjectService projectService, UserManager<AppUser> userService, IAppUserService appUserService, IProjectUserService projectUserService, ITaskService taskService, ITagService tagService)
		{
			_projectService = projectService;
			_userService = userService;
			_appUserService = appUserService;
			_projectUserService = projectUserService;
			_taskService = taskService;
			_tagService = tagService;
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
			
            
            ViewData["Projects"] = values;
            ViewData["Selects"] = SelectList(null);


            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(AddProjectViewModel addProjectViewModel)
        {

            Project project = new Project()
            {
                Name = addProjectViewModel.Name,
                Description = addProjectViewModel.Description,
                CommanderId = addProjectViewModel.CommanderId,
            }; // project create


            var NewProject = _projectService.Insert(project);

            _projectUserService.CreateProjectUserList(addProjectViewModel.UserIds, NewProject.Id);

			return RedirectToAction("Index");
        }


		[Authorize]
		public IActionResult ProjectDetails(int id)
		{

			var tasks = _taskService.GetAllByProjectId(id);
            List<AllTaskViewModel> allTasks = new List<AllTaskViewModel>();
            List<AllTagsViewModel> allTags = new List<AllTagsViewModel>();

            foreach(var task in tasks)
            {
                AllTaskViewModel taskViewModel = new AllTaskViewModel();
                taskViewModel.Priority = task.Priority;
                taskViewModel.Responsible = task.Responsible.UserName;
                taskViewModel.Reporter = task.Reporter.UserName;
                taskViewModel.DueTime = task.DueDate;
                taskViewModel.Name = task.Name;
                taskViewModel.Description = task.Description;
                taskViewModel.Tag = task.Tag.Name;

                allTasks.Add(taskViewModel);
            } // allTasks create

            foreach(var tag in _tagService.GetAll())
            {
                AllTagsViewModel allTagsViewModel = new AllTagsViewModel();
                allTagsViewModel.Name = tag.Name;
                allTagsViewModel.Id = tag.Id;

                allTags.Add(allTagsViewModel);
            } //alltags create


            ViewData["AllTasks"] = allTasks;
            ViewData["Selects"] = SelectList(null);
            ViewData["Tags"] = allTags;
            ViewData["id"] = id;

            



			return View();
		}


		[Authorize]
        [HttpPost]
		public IActionResult ProjectDetails(AddTaskViewModel addTaskViewModel)
        {

            Task task = new Task()
            {
                Name = addTaskViewModel.Name,
                Description = addTaskViewModel.Description,
                Priority = addTaskViewModel.Priority,
                ResponsibleId = addTaskViewModel.ResponsibleId,
                ReporterId = addTaskViewModel.ReporterId,
                DueDate = addTaskViewModel.DueTime.ToUniversalTime(),
                ProjectId = addTaskViewModel.ProjectId,
                TagId = addTaskViewModel.TagId
                

                
            }; // task create

            // şu araya fluent validation eklenebilir

            _taskService.Insert(task);

            return RedirectToAction("ProjectDetails","Project", new { id = addTaskViewModel.ProjectId });




            // validation kontrolü yapıldıktan sonra ihtiyac olacak




            return View();
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
            ValidationResult result = validationRules.Validate(project); 
            if (result.IsValid)
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


		public List<SelectListItemViewModel> SelectList(string keyword)
		{
			return _appUserService.GetUsersByKeyword(keyword).Select(x => new SelectListItemViewModel()
			{
				id = x.Id,
				text = x.UserName
			}).ToList();


		}
	}
}