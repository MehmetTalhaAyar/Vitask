using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Business.Abstract;
using Business.Models;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Vitask.Models;
using Vitask.Statics;
using Task = Entities.Concrete.Task;

namespace Vitask.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        private readonly UserManager<AppUser> _userService;

        private readonly IAppUserService _appUserService;

        private readonly ITagService _tagService;

        private readonly ICommentService _commentService;

        private readonly IMapper _mapper;

		public TaskController(ITaskService taskService, UserManager<AppUser> userService, IAppUserService appUserService, ITagService tagService, ICommentService commentService, IMapper mapper)
		{
			_taskService = taskService;
			_userService = userService;
			_appUserService = appUserService;
			_tagService = tagService;
			_commentService = commentService;
			_mapper = mapper;
		}

		[Authorize]
        public async Task<IActionResult> Index(int page = 1)
        {

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var allTasks = new List<AllMyTaskViewModel>();

           


            var pageCount = _taskService.GetPageCountByUserId(userId);

            if (page < 1 || page > pageCount)
                page = 1;

            List<Task> tasks;

            string key = $"Tasks_Index_{page}";

            if(!CacheManager.TryGetValue(key,out allTasks))
            {
                tasks = _taskService.GetAllByResponsibleId(userId, page);

				allTasks = _mapper.Map<List<AllMyTaskViewModel>>(tasks);

				if (allTasks != null)
                {
                    CacheManager.AddToCache(key, allTasks, TimeSpan.FromMinutes(10),"Task");
                }
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
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var task = _taskService.GetTaskWithRelations(id);

            var taskVms = _mapper.Map<TaskViewModel>(task);

			var tags = _tagService.GetAll();

            List<AllTagsViewModel> allTags = _mapper.Map<List<AllTagsViewModel>>(tags);

			var selects = _appUserService.SelectList(null,task.ProjectId, new List<int>() { task.Reporter.Id, task.Responsible.Id });

            ViewData["Selects"] = selects;
            ViewData["Tags"] = allTags;
            ViewData["UserId"] = userId;
            ViewData["TaskModel"] = taskVms;

            UpdateTaskViewModel updateTaskViewModel = _mapper.Map<UpdateTaskViewModel>(task);

            return View(updateTaskViewModel);
		}

		[HttpPost]
		[Authorize]
		public IActionResult TaskDetails(UpdateTaskViewModel updateTaskViewModel)
        {

            var task = _taskService.GetById(updateTaskViewModel.Id);

            Task updatedTask = _mapper.Map<Task>(updateTaskViewModel); 
            updatedTask.ProjectId = task.ProjectId;

            TaskValidator validationRules = new TaskValidator();
            ValidationResult result = validationRules.Validate(updatedTask);

            if (result.IsValid)
            {
                _taskService.Update(updatedTask);
                CacheManager.RemoveByGroup("Task");

                return RedirectToAction("TaskDetails","Task",new {id = updateTaskViewModel.Id});
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }


            return RedirectToAction("TaskDetails","Task",new { id = updateTaskViewModel.Id});
        }

        [Authorize]
		public IActionResult DeleteTask(int id)
		{
			var value = _taskService.GetById(id);
			_taskService.Delete(value);
			CacheManager.RemoveByGroup("Task");
			return RedirectToAction("Index");
		}





		
    }
}
