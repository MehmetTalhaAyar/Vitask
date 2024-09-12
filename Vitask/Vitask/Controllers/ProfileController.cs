using System.Security.Claims;
using Business.Abstract;
using Business.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vitask.Models;

namespace Vitask.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userService;

        private readonly IAppUserService _appUserService;

        private readonly IUserInfoService _userInfoService;

		public ProfileController(UserManager<AppUser> userService, IAppUserService appUserService, IUserInfoService userInfoService)
		{
			_userService = userService;
			_appUserService = appUserService;
			_userInfoService = userInfoService;
		}

		[Authorize]
        [HttpGet("/Profile/{userName}")]
        public async Task<IActionResult> Index([FromRoute] string userName)
        {

            var value = _appUserService.GetByUsernameWithUserInfo(userName);

            ProfileViewModel model = new ProfileViewModel()
            {
                Name = value.Name,
                Surname = value.Surname,
                Email = value.Email,
                PictureUrl = value.Image,
                Username = value.UserName,
                About = value.UserInfo != null ? value.UserInfo.About : null,
                Location = value.UserInfo != null ? value.UserInfo.Location : null,
                Title = value.UserInfo != null ? value.UserInfo.Title : null
            };

            return View(model);
        }


        [Authorize]
        [HttpGet("/MyProfile/AccountSettings")]
        public IActionResult AccountSettings()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var user = _appUserService.GetByIdWithUserInfo(userId);

            if (user == null)
                return RedirectToAction("Index", "Login");

            

            ProfileViewModel model = new ProfileViewModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PictureUrl = user.Image,
                Username = user.UserName,
                About = user.UserInfo != null ? user.UserInfo.About : null,
                Location = user.UserInfo != null ? user.UserInfo.Location : null,
                Title = user.UserInfo != null ? user.UserInfo.Title : null  
            };

            ViewData["User"] = model;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult UserInfos(UpdateUserInfoViewModel updateUserInfoViewModel)
        {
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = _appUserService.GetByIdWithUserInfo(userId);

            if (user == null)
                return RedirectToAction("Index", "Login");


            if(user.UserInfo == null)
            {

                UserInfo userInfo = new UserInfo()
                {
                    About = updateUserInfoViewModel.About,
                    Location = updateUserInfoViewModel.Location,
                    Title = updateUserInfoViewModel.Title,
                    UserId = userId

                };


                _userInfoService.Insert(userInfo);

            }
            else
            {
                UserInfo userInfo = user.UserInfo;

                userInfo.Title = updateUserInfoViewModel.Title;
                userInfo.Location = updateUserInfoViewModel.Location;
                userInfo.About = updateUserInfoViewModel.About;

                _userInfoService.Update(userInfo);
            }



			return RedirectToAction($"{user.UserName}", "Profile");
        }



    }
}
