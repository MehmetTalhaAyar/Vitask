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
using AutoMapper;
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

        private readonly IMapper _mapper;

		public ProjectController(IProjectService projectService, UserManager<AppUser> userService, IAppUserService appUserService, IProjectUserService projectUserService, ITaskService taskService, ITagService tagService, IMapper mapper)
		{
			_projectService = projectService;
			_userService = userService;
			_appUserService = appUserService;
			_projectUserService = projectUserService;
			_taskService = taskService;
			_tagService = tagService;
			_mapper = mapper;
		}

		[Authorize]
        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _userService.GetUserAsync(User);
            
            if(user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int pageCount;

            if (User.IsInRole("Admin"))
            {
                pageCount = _projectService.GetPageCount();
            }
            else
            {
                pageCount = _projectService.GetPageCount(user.Id);

            }

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


                models = _mapper.Map<List<ProjectViewModel>>(values);

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

            Project project = _mapper.Map<Project>(addProjectViewModel);

            // buraya fluent validation eklenecek

            ProjectValidator validationRules = new ProjectValidator();
            ValidationResult result = validationRules.Validate(project);

            if (result.IsValid)
            {
				if (!addProjectViewModel.UserIds.Contains(addProjectViewModel.CommanderId)) // proje lideri her zaman projenin içindedir
					addProjectViewModel.UserIds.Add(addProjectViewModel.CommanderId);

				var NewProject = _projectService.Insert(project);

				_projectUserService.CreateProjectUserList(addProjectViewModel.UserIds, NewProject.Id);


				CacheManager.RemoveByGroup("Project");

				return RedirectToAction("Index");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            return View(addProjectViewModel);
        }


		[Authorize]
		public IActionResult ProjectDetails(int id,int page = 1)
		{
            var pageCount = _taskService.GetPageCount(id);

			if (page < 1 || page > pageCount)
				page = 1;

            List<Task> tasks;
            List<Tag> tags;

            List<AllTaskViewModel> allTasks;
            List<AllTagsViewModel> allTags;

			string cacheKeyTask = $"Project_ProjectDetails_Tasks_{id}_{page}";

            string cacheKeyTag = "Tags_All";
            
            if(!CacheManager.TryGetValue(cacheKeyTask,out allTasks))
            {

			    tasks = _taskService.GetAllByProjectId(id,page);

                allTasks = _mapper.Map<List<AllTaskViewModel>>(tasks);

                if(allTasks != null)
                {
                    CacheManager.AddToCache(cacheKeyTask, allTasks, TimeSpan.FromMinutes(10), "Task");
                }

            }

            if(!CacheManager.TryGetValue(cacheKeyTag,out allTags))
            {
                tags = _tagService.GetAll();

                allTags = _mapper.Map<List<AllTagsViewModel>>(tags);


				if (allTags != null)
                {
                    CacheManager.AddToCache(cacheKeyTag, allTags, TimeSpan.FromMinutes(10), "Tag");
                }
            }

            
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

            Task task = _mapper.Map<Task>(addTaskViewModel);

            TaskValidator validationRules = new TaskValidator();
            ValidationResult result = validationRules.Validate(task);

            if (result.IsValid)
            {
				_taskService.Insert(task);
				var pageCount = _taskService.GetPageCount(addTaskViewModel.ProjectId);


				CacheManager.RemoveByGroup("Task");

				return RedirectToAction("ProjectDetails", "Project", new { id = addTaskViewModel.ProjectId });
			}else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            return RedirectToAction("ProjectDetails","Project");
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

            UpdateProjectViewModel updateProjectViewModel;

			string cacheKey = $"Project_EditProject_{id}";


            if(!CacheManager.TryGetValue(cacheKey,out updateProjectViewModel))
            {

                project = _projectService.GetByIdWithRelations(id);

                updateProjectViewModel = _mapper.Map<UpdateProjectViewModel>(project);

                if(updateProjectViewModel != null)
                {
                    CacheManager.AddToCache(cacheKey, updateProjectViewModel, TimeSpan.FromMinutes(10),"Project");
                }


            }

            var selects = _appUserService.SelectList(null, null, updateProjectViewModel.UserIds);

            ViewData["Selects"] = selects;


            return View(updateProjectViewModel);
        }

		[Authorize]
		[HttpPost]
        public IActionResult EditProject(UpdateProjectViewModel updateProjectViewModel)
        {


            Project project = _mapper.Map<Project>(updateProjectViewModel);
            project.UpdatedOn = DateTime.Now.ToUniversalTime();

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
			
            ViewData["Selects"] = _appUserService.SelectList(null, null, updateProjectViewModel.UserIds);

			return View(updateProjectViewModel);
        }



        
       

		}
}