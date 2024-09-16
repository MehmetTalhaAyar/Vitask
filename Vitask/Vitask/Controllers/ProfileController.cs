using System.Security.Claims;
using AutoMapper;
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

        private readonly IMapper _mapper;

		public ProfileController(UserManager<AppUser> userService, IAppUserService appUserService, IUserInfoService userInfoService, IMapper mapper)
		{
			_userService = userService;
			_appUserService = appUserService;
			_userInfoService = userInfoService;
			_mapper = mapper;
		}

		[Authorize]
        [HttpGet("/Profile/{userName}")]
        public async Task<IActionResult> Index([FromRoute] string userName)
        {

            var value = _appUserService.GetByUsernameWithUserInfo(userName);

            ProfileViewModel model = _mapper.Map<ProfileViewModel>(value);

            return View(model);
        }


        [Authorize]
        [HttpGet("/MyProfile/AccountSettings")]
        public IActionResult AccountSettings()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var user = _appUserService.GetByIdWithUserInfo(userId);

            ProfileViewModel model = _mapper.Map<ProfileViewModel>(user);

            ViewData["User"] = model;

            return View();
        }

        [Authorize]
        [HttpGet("/MyProfile/SecuritySettings")]
        public  IActionResult SecuritySettings()
        {
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            var user = _appUserService.GetById(userId);

            var model = _mapper.Map<UserViewModel>(user);


            ViewData["User"] = model;

			return View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult UserInfos(UpdateUserInfoViewModel updateUserInfoViewModel)
        {
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = _appUserService.GetByIdWithUserInfo(userId);

            if(user.UserInfo == null)
            {

                UserInfo userInfo = _mapper.Map<UserInfo>(updateUserInfoViewModel);
                userInfo.UserId = userId;


                _userInfoService.Insert(userInfo);

            }
            else
            {
                UserInfo userInfo;

                userInfo = _mapper.Map<UserInfo>(updateUserInfoViewModel);
                userInfo.UserId = user.Id;
                userInfo.Id = user.UserInfo.Id;

                _userInfoService.Update(userInfo);
            }



			return RedirectToAction($"{user.UserName}", "Profile");
        }



    }
}
