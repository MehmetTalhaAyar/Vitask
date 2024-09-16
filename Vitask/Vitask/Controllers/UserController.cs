using AutoMapper;
using Business.Abstract;
using Business.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Vitask.Models;

namespace Vitask.Controllers
{
    public class UserController : Controller
    {
		private readonly IAppUserService _appUserService;

		private readonly UserManager<AppUser> _userService;

		private readonly IMapper _mapper;

		public UserController(IAppUserService appUserService, UserManager<AppUser> userService, IMapper mapper)
		{
			_appUserService = appUserService;
			_userService = userService;
			_mapper = mapper;
		}

		[Authorize(Roles ="Admin")]
		public async Task<IActionResult> Index(int Page = 1)
		{

			var user = await _userService.GetUserAsync(User);

			if(user == null)
			{
				return RedirectToAction("Index");
			}

			var pageCount = _appUserService.GetPageCount(user.Id);

			if (Page < 1 || Page > pageCount)
				Page = 1;

			var userList = _appUserService.GetAllUsers(Page,user.Id);

			List<UserViewModel> users = _mapper.Map<List<UserViewModel>>(userList);


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

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> UserCreate(SignUpViewModel signUpViewModel)
		{

			AppUser user = _mapper.Map<AppUser>(signUpViewModel);


			var result = await _userService.CreateAsync(user, signUpViewModel.Password);

			if (result.Succeeded)
			{
				await _userService.AddToRoleAsync(user, "User");
			}

			return RedirectToAction("Index");
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> ChangeSecurity(EditUserSecurityModel userSecurityModel)
		{

			var user = await _userService.GetUserAsync(User);

			user.Email = userSecurityModel.Email;
			user.UserName = userSecurityModel.Username;

			var token = await _userService.GeneratePasswordResetTokenAsync(user);
			var result = await _userService.ResetPasswordAsync(user, token, userSecurityModel.Password);

			if (result.Succeeded)
				return RedirectToAction("Logout", "Login");



			return RedirectToAction("Index","Dashboard");
		}



		[Authorize]
		public List<SelectListItemViewModel> SelectList(string keyword, int? ProjectId, List<int>? selectedUsers = null)
		{
			return _appUserService.SelectList(keyword,ProjectId, selectedUsers);
		}

		

	}
}
