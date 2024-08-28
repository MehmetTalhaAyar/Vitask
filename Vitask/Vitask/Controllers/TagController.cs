using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Vitask.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }
        public IActionResult Index()
        {
            var values = _tagService.GetAll();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddTag()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTag(Tag tag)
        {
            TagValidator validationRules = new TagValidator();
            ValidationResult result = validationRules.Validate(tag);
            if (result.IsValid)
            {
                _tagService.Insert(tag);
                return RedirectToAction("Index");
            }
            else
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
            
        }
        public IActionResult DeleteTag(int id)
        {
            var value = _tagService.GetById(id);
            _tagService.Delete(value);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditTag(int id)
        {
            var value = _tagService.GetById(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult EditTag(Tag tag)
        {
            TagValidator validationRules = new TagValidator();
            ValidationResult result = validationRules.Validate(tag);
            if (result.IsValid)
            {
                _tagService.Update(tag);
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
