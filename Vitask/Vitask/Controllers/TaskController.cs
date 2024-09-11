using System.Security.Claims;
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

		public TaskController(ITaskService taskService, UserManager<AppUser> userService, IAppUserService appUserService, ITagService tagService, ICommentService commentService)
		{
			_taskService = taskService;
			_userService = userService;
			_appUserService = appUserService;
			_tagService = tagService;
			_commentService = commentService;
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

            if(!CacheManager.TryGetValue(key,out tasks))
            {
                tasks = _taskService.GetAllByResponsibleId(userId, page);

                if(tasks != null)
                {
                    CacheManager.AddToCache(key, tasks,TimeSpan.FromMinutes(10),"Task");
                }
            }




            foreach (var task in tasks)
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
			int? userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			if (userId == null)
                return RedirectToAction("Index", "Login");

            

            var task = _taskService.GetTaskWithRelations(id);


            var taskComments = _commentService.GetAllByTaskId(task.Id);

            var Ids = taskComments.Select(x => x.Id).ToList();

            Dictionary<int, List<CommentViewModel>?> replysDict = new Dictionary<int, List<CommentViewModel>?>();

            foreach(var item in Ids)
            {

                var replys = _commentService.GetAllReplys(item);
                if(replys == null)
                {
                    replysDict[item] = null;
                }
                else
                {
                    List<int> likedCommentsIds = replys.Select(x=> x.Likes.Where(y=> y.UserId == userId).FirstOrDefault()).Where(x=> x != null).Select(x=>x.CommentId).ToList();

                    List<CommentViewModel> replyComments = replys.Select(x => new CommentViewModel()
                    {
                        Id = x.Id,
                        Content = x.Content,
                        User = new UserViewModel()
                        {
                            Id = x.User.Id,
                            Email = x.User.Email,
                            Name = x.User.Name,
                            LastName = x.User.Surname,
                            Username = x.User.UserName,
                            Image = x.User.Image
                        },
                        CreatedOn = x.CreatedOn,
                        isLike = likedCommentsIds.Contains(x.Id),
                        Replys = null
                    }).ToList();

                    replysDict[item] = replyComments;

                }


            } 


            List<int> likedCommentsId = taskComments.Select(x => x.Likes.Where(z => z.UserId == userId).FirstOrDefault()).Where(x=> x != null).Select(x => x.CommentId).ToList();

            List<CommentViewModel> comments = taskComments.Select(x => new CommentViewModel()
            {
                Id = x.Id,
                Content = x.Content,
                User = new UserViewModel()
                {
                    Id = x.User.Id,
                    Email = x.User.Email,
                    LastName = x.User.Surname,
                    Name = x.User.Name,
                    Username = x.User.UserName,
                    Image = x.User.Image
                },
                CreatedOn = x.CreatedOn,
                isLike = likedCommentsId.Contains(x.Id),
                Replys = replysDict[x.Id]
            }).ToList();



            #region Model Mapping işlemleri
            List<AllTagsViewModel> allTags = new List<AllTagsViewModel>();

            UserViewModel reporter = new UserViewModel()
            {
                Id = task.Reporter.Id,
                Name = task.Reporter.Name,
                LastName = task.Reporter.Surname,
                Email = task.Reporter.Email,
                Username = task.Reporter.UserName,
                Image = task.Reporter.Image
            }; 

            UserViewModel responsible = new UserViewModel()
            {
                Id = task.Responsible.Id,
                Name = task.Responsible.Name,
                LastName = task.Responsible.Surname,
                Email = task.Responsible.Email,
                Username = task.Responsible.UserName,
                Image = task.Responsible.Image
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
                ResponsibleId = responsible,
                Comments = comments
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
            ViewData["UserId"] = userId;
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
            CacheManager.RemoveByGroup("Task");
            return RedirectToAction("TaskDetails","Task",new {id = updateTaskViewModel.Id});
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
