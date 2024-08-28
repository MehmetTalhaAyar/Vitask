using Business.Abstract;
using Business.ValidationRules;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Task = Entities.Concrete.Task;

namespace Vitask.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        public IActionResult Index()
        {
            var values = _taskService.GetAll();
            return View(values);
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
