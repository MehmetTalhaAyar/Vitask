using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vitask.Statics;

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
            List<Tag> tags;

            string key = "Tags_All";

            if(!CacheManager.TryGetValue(key,out tags))
            {

                tags = _tagService.GetAll();

                if(tags != null)
                {
                    CacheManager.AddToCache(key, tags, TimeSpan.FromMinutes(10), "Tag");
                }

            }
            
            return View(tags);
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
                CacheManager.RemoveByGroup("Tag");
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
            CacheManager.RemoveByGroup("Tag");
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
                CacheManager.RemoveByGroup("Tag");
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
