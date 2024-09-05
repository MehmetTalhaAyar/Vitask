using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index()
        {
            var values = _tagService.GetAll();
            return View(values);
        }


		[Authorize(Roles = "Admin")]
		[HttpGet]
        public IActionResult AddTag()
        {
            return View();
        }

		[Authorize(Roles = "Admin")]
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

		[Authorize(Roles = "Admin")]
		public IActionResult DeleteTag(int id)
        {
            var value = _tagService.GetById(id);
            _tagService.Delete(value);
            return RedirectToAction("Index");
        }

		[Authorize(Roles = "Admin")]
		[HttpGet]
        public IActionResult EditTag(int id)
        {
            var value = _tagService.GetById(id);
            return View(value);
        }

		[Authorize(Roles = "Admin")]
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
