using Business.Abstract;
using Business.Models;
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
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        private readonly UserManager<AppUser> _userService;

        private readonly IAppUserService _appUserService;

        private readonly ITagService _tagService;

		public TaskController(ITaskService taskService, UserManager<AppUser> userService, IAppUserService appUserService, ITagService tagService)
		{
			_taskService = taskService;
			_userService = userService;
			_appUserService = appUserService;
			_tagService = tagService;
		}

		[Authorize]
        public async Task<IActionResult> Index(int page = 1)
        {

            var user = await _userService.GetUserAsync(User);

            if(user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var allTasks = new List<AllMyTaskViewModel>();

           


            var pageCount = _taskService.GetPageCountByUserId(user.Id);

            if (page < 1 || page > pageCount)
                page = 1;


			foreach (var task in _taskService.GetAllByResponsibleId(user.Id, page))
            {
                AllMyTaskViewModel taskViewModel = new AllMyTaskViewModel()
                {
                    Id = task.Id,
                    Description = task.Description,
                    DueTime = task.DueDate,
                    Name = task.Name,
                    Priority = task.Priority,
                    ProjectName = task.Project.Name,
                    Reporter = task.Reporter.UserName,
                    Responsible = task.Responsible.UserName,
                    Tag = task.Tag.Name
                };

                allTasks.Add(taskViewModel);
            }

				PageInfoModel pageInfoModel = new PageInfoModel()
            {
                CurrentPage = page,
                PageCount = pageCount
            };

            ViewData["PageInfo"] = pageInfoModel;
            ViewData["AllTasks"] = allTasks;

            return View();
        }

		[HttpGet]
        [Authorize]
		public IActionResult TaskDetails(int id)
		{
            var task = _taskService.GetTaskWithRelations(id);


            #region Model Mapping işlemleri
            List<AllTagsViewModel> allTags = new List<AllTagsViewModel>();

            UserViewModel reporter = new UserViewModel()
            {
                Id = task.Reporter.Id,
                Name = task.Reporter.Name,
                LastName = task.Reporter.Surname,
                Email = task.Reporter.Email,
                Username = task.Reporter.UserName
            }; 

            UserViewModel responsible = new UserViewModel()
            {
                Id = task.Responsible.Id,
                Name = task.Responsible.Name,
                LastName = task.Responsible.Surname,
                Email = task.Responsible.Email,
                Username = task.Responsible.UserName
            };

            ProjectViewModel project = new ProjectViewModel()
            {
                Id = task.Project.Id,
                Name = task.Project.Name
            };

            AllTagsViewModel tag = new AllTagsViewModel()
            {
               Id = task.Tag.Id,
               Name = task.Tag.Name
            };

            TaskViewModel TaskViewModel = new TaskViewModel()
            {
                Id=task.Id,
                Description = task.Description,
                DueTime = task.DueDate,
                Name = task.Name,
                CreatedOn = task.CreatedOn,
                Project = project,
                Status = task.Status,
                Tag = tag,
                ReporterId = reporter,
                ResponsibleId = responsible
            };

			var tags = _tagService.GetAll();
            
            foreach(var item in tags)
            {
                AllTagsViewModel tagsViewModel = new AllTagsViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                };

                allTags.Add(tagsViewModel);
            }

			#endregion

			var selects = _appUserService.SelectList(null,task.ProjectId, new List<int>() { reporter.Id, responsible.Id });
            ViewData["Selects"] = selects;
            
            ViewData["Tags"] = allTags;

            ViewData["TaskModel"] = TaskViewModel;

            UpdateTaskViewModel updateTaskViewModel = new UpdateTaskViewModel()
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                DueTime= task.DueDate,
                Priority = task.Priority,
                ReporterId= reporter.Id,
                ResponsibleId= responsible.Id,
                TagId = tag.Id,
            };


            return View(updateTaskViewModel);
		}

		[HttpPost]
		[Authorize]
		public IActionResult TaskDetails(UpdateTaskViewModel updateTaskViewModel)
        {

            var task = _taskService.GetById(updateTaskViewModel.Id);
            task.Name = updateTaskViewModel.Name;
            task.Description = updateTaskViewModel.Description;
            task.UpdatedOn = DateTime.Now.ToUniversalTime();
            task.TagId = updateTaskViewModel.TagId;
            task.ReporterId = updateTaskViewModel.ReporterId;
            task.DueDate = updateTaskViewModel.DueTime.ToUniversalTime();
            task.ResponsibleId = updateTaskViewModel.ResponsibleId;
            task.Priority = updateTaskViewModel.Priority;
            
            ///burada validation yapılacak
            
            _taskService.Update(task);
            
            return RedirectToAction("Index");
        }





		[HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTask(Task task)
        {
            TaskValidator validationRules = new TaskValidator();
            ValidationResult result = validationRules.Validate(task);
            if (result.IsValid)
            {
                _taskService.Insert(task);
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
        public IActionResult DeleteTask(int id)
        {
            var value = _taskService.GetById(id);
            _taskService.Delete(value);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditTask(int id)
        {
            var value = _taskService.GetById(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult EditTask(Task task)
        {
            TaskValidator validationRules = new TaskValidator();
            ValidationResult result = validationRules.Validate(task);
            if (result.IsValid)
            {
                _taskService.Update(task);
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
