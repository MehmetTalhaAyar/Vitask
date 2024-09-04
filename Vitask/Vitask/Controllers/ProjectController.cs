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

            // buraya fluent validation eklenecek


            var NewProject = _projectService.Insert(project);

            _projectUserService.CreateProjectUserList(addProjectViewModel.UserIds, NewProject.Id);

			return RedirectToAction("Index");
        }


		[Authorize]
		public IActionResult ProjectDetails(int id,int page = 1)
		{
            var pageCount = _taskService.GetPageCount(id);

			if (page < 1 || page > pageCount)
				page = 1;

			var tasks = _taskService.GetAllByProjectId(id,page);
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

            PageInfoModel pageInfo = new PageInfoModel()
            {
                CurrentPage = page,
                PageCount = pageCount
            };

            ViewData["AllTasks"] = allTasks;
            ViewData["Selects"] = SelectList(null);
            ViewData["Tags"] = allTags;
            ViewData["id"] = id;
            ViewData["PageInfo"] = pageInfo;






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

            // buraya fluent validation eklenecek

            _taskService.Insert(task);

            return RedirectToAction("ProjectDetails", "Project", new { id = addTaskViewModel.ProjectId });
        }


		[Authorize]
		public IActionResult DeleteProject(int id) // burası soft delete işlemine çevrilecek
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

            UpdateProjectViewModel updateProjectViewModel = new UpdateProjectViewModel()
            {
                Id = value.Id,
                Name = value.Name,
                Description = value.Description,
                CommanderId = value.CommanderId,
                UserIds = _projectUserService.GetUserIdByProject(value.Id)
            };


            ViewData["Selects"] = SelectList(null, updateProjectViewModel.UserIds);


            return View(updateProjectViewModel);
        }

		[Authorize]
		[HttpPost]
        public IActionResult EditProject(UpdateProjectViewModel updateProjectViewModel)
        {


			Project project = new Project()
			{
                Id = updateProjectViewModel.Id,
				Name = updateProjectViewModel.Name,
				Description = updateProjectViewModel.Description,
				CommanderId = updateProjectViewModel.CommanderId,
                UpdatedOn = DateTime.Now.ToUniversalTime()
			};

			ProjectValidator validationRules = new ProjectValidator();
            ValidationResult result = validationRules.Validate(project);
            if (result.IsValid)
            {
                _projectService.Update(project);
                _projectUserService.UpdateProjectUserList(updateProjectViewModel.UserIds, project.Id);

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


		public List<SelectListItemViewModel> SelectList(string keyword,List<int>? selectedUsers = null)
		{
            var selectList = _appUserService.GetUsersByKeyword(keyword).Select(x => new SelectListItemViewModel()
			{
				id = x.Id,
				text = x.UserName
			}).ToList();

            if (selectedUsers != null)
            {
                var selectListIds = selectList.Select(x => x.id).ToList();
                var goingtoadds = selectedUsers.Where(x => !selectListIds.Contains(x));

                foreach (var item in goingtoadds)
                {
                    var user = _appUserService.GetById(item);
                    if (user != null)
                    {
                        SelectListItemViewModel selectListItemViewModel = new SelectListItemViewModel()
                        {
                            id = user.Id,
                            text = user.UserName
                        };
                        selectList.Add(selectListItemViewModel);
                    }
                }
            }




            return selectList;


		}
	}
}