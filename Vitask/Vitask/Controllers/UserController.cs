using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vitask.Models;

namespace Vitask.Controllers
{
    public class UserController : Controller
    {
		private readonly IAppUserService _appUserService;

		private readonly UserManager<AppUser> _userService;

		public UserController(IAppUserService appUserService, UserManager<AppUser> userService)
		{
			_appUserService = appUserService;
			_userService = userService;
		}

		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> Index(int Page = 1)
		{

			var user = await _userService.GetUserAsync(User);

			if(user == null)
			{
				return RedirectToAction("Index");
			}
			List<UserViewModel> users = new List<UserViewModel>();

			var pageCount = _appUserService.GetPageCount(user.Id);

			if (Page < 1 || Page > pageCount)
				Page = 1;

			var userList = _appUserService.GetAllUsers(Page,user.Id);

			foreach(var item in userList)
			{
				UserViewModel userViewModel = new UserViewModel()
				{
					Id = item.Id,
					Email = item.Email,
					LastName = item.Surname,
					Name = item.Name,
					Username = item.UserName
				};

				users.Add(userViewModel);
				
			}

			PageInfoModel pageInfoModel = new PageInfoModel()
			{
				CurrentPage = Page,
				PageCount = pageCount,
			};
			ViewData["PageInfo"] = pageInfoModel;
			ViewData["Users"] = users;

			return View();
		}


		[Authorize(Roles ="Admin")]
		public IActionResult UserDelete(int id)
		{

			_appUserService.Delete(id);

			return RedirectToAction("Index");
		}


		[Authorize]
		public List<SelectListItemViewModel> SelectList(string keyword, int? ProjectId, List<int>? selectedUsers = null)
		{
			return _appUserService.SelectList(keyword,ProjectId, selectedUsers);
		}

		

	}
}
