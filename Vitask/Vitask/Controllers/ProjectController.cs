using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task = Entities.Concrete.Task;
using Vitask.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Caching.Memory;
using Vitask.Statics;
using Business.Models;
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
        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _userService.GetUserAsync(User);
            
            if(user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var pageCount = _projectService.GetPageCount();

            if (page < 1 || page > pageCount)
                page = 1;

            List<ProjectViewModel> models;
            List<Project> values;

            string cacheKey = $"Project_Index_{page}";

            if(!CacheManager.TryGetValue(cacheKey,out models)){

                if (User.IsInRole("Admin"))
                {
                    values = _projectService.GetAllWithCommander(page);
                }
                else
                {
                    values = _projectService.GetAllByUserId(user.Id,page);
                }


                models = values.Select(x => new ProjectViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Leader = new UserViewModel()
                    {
                        Id = x.Commander.Id,
                        Email = x.Commander.Email,
                        Image = x.Commander.Image,
                        LastName = x.Commander.Surname,
                        Name = x.Commander.Name,
                        Username = x.Commander.UserName
                    }
                }).ToList();




                if(models != null)
                {
                    CacheManager.AddToCache(cacheKey, models, TimeSpan.FromMinutes(10),"Project");
                }
            }
            PageInfoModel pageInfoModel = new PageInfoModel()
            {
                CurrentPage = page,
                PageCount = pageCount
            };


            ViewData["PageInfo"] = pageInfoModel;
            ViewData["Projects"] = models;
            ViewData["Selects"] = _appUserService.SelectList(null,null,null);

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

            string cacheKey = "Project_Index";

            if (!addProjectViewModel.UserIds.Contains(addProjectViewModel.CommanderId)) // proje lideri her zaman projenin içindedir
                addProjectViewModel.UserIds.Add(addProjectViewModel.CommanderId);

            var NewProject = _projectService.Insert(project);

            _projectUserService.CreateProjectUserList(addProjectViewModel.UserIds, NewProject.Id);


            CacheManager.RemoveByGroup("Project");

			return RedirectToAction("Index");
        }


		[Authorize]
		public IActionResult ProjectDetails(int id,int page = 1)
		{
            var pageCount = _taskService.GetPageCount(id);

			if (page < 1 || page > pageCount)
				page = 1;

            List<Task> tasks;
            List<Tag> tags;

            string cacheKeyTask = $"Project_ProjectDetails_Tasks_{id}_{page}";

            string cacheKeyTag = "Tags_All";
            
            if(!CacheManager.TryGetValue(cacheKeyTask,out tasks))
            {

			    tasks = _taskService.GetAllByProjectId(id,page);

                if(tasks != null)
                {
                    CacheManager.AddToCache(cacheKeyTask, tasks, TimeSpan.FromMinutes(10), "Task");
                }

            }

            if(!CacheManager.TryGetValue(cacheKeyTag,out tags))
            {
                tags = _tagService.GetAll();

                if(tags != null)
                {
                    CacheManager.AddToCache(cacheKeyTag, tags, TimeSpan.FromMinutes(10), "Tag");
                }
            }


            List<AllTaskViewModel> allTasks = new List<AllTaskViewModel>();
            List<AllTagsViewModel> allTags = new List<AllTagsViewModel>();

            
            //buralar automapper konulduktan sonra cache içine alınacak
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
                taskViewModel.Id = task.Id;

                allTasks.Add(taskViewModel);
            } // allTasks create

            foreach(var tag in tags)
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
            ViewData["Selects"] = _appUserService.SelectList(null,id,null);
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
            var pageCount = _taskService.GetPageCount(addTaskViewModel.ProjectId);


            CacheManager.RemoveByGroup("Task");

            return RedirectToAction("ProjectDetails", "Project", new { id = addTaskViewModel.ProjectId });
        }


		[Authorize]
		public IActionResult DeleteProject(int id) 
        {
            var value = _projectService.GetByIdWithTasks(id);
            
            foreach(var item in value.Tasks)
            {
                _taskService.Delete(item);
            }

            _projectService.Delete(value);

            CacheManager.RemoveByGroup("Project");
            CacheManager.RemoveByGroup("Task");
            return RedirectToAction("Index");
        }


		[Authorize]
		[HttpGet]
        public IActionResult EditProject(int id)
        {
            Project project;
            List<int> userIds;


            string cacheKey = $"Project_EditProject_{id}";

            string cacheKeyUserIds = $"Project_EditProject_UserIds_{id}";

            if(!CacheManager.TryGetValue(cacheKey,out project))
            {

                project = _projectService.GetById(id);

                if(project != null)
                {
                    CacheManager.AddToCache(cacheKey,project,TimeSpan.FromMinutes(10),"Project");
                }


            }

            if(!CacheManager.TryGetValue(cacheKeyUserIds,out userIds))
            {
                userIds = _projectUserService.GetUserIdByProject(project.Id);
                userIds.Add(project.CommanderId);

                CacheManager.AddToCache(cacheKeyUserIds, userIds, TimeSpan.FromMinutes(10), "Project");
            }

            UpdateProjectViewModel updateProjectViewModel = new UpdateProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CommanderId = project.CommanderId,
                UserIds = userIds
            };

            var selects = _appUserService.SelectList(null, null, updateProjectViewModel.UserIds);

            ViewData["Selects"] = selects;


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
                if (!updateProjectViewModel.UserIds.Contains(updateProjectViewModel.CommanderId)) 
                    updateProjectViewModel.UserIds.Add(updateProjectViewModel.CommanderId);


                _projectService.Update(project);
                _projectUserService.UpdateProjectUserList(updateProjectViewModel.UserIds, project.Id);

                CacheManager.RemoveByGroup("Project");

                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
				}

            }
			var userIds = _projectUserService.GetUserIdByProject(updateProjectViewModel.Id);
            ViewData["Selects"] = _appUserService.SelectList(null, null, updateProjectViewModel.UserIds);

			return View(updateProjectViewModel);
        }



        
       

		}
}