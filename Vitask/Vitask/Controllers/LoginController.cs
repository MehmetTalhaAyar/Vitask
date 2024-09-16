using System.Security.Claims;
using AutoMapper;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vitask.Models;
using Vitask.Statics;

namespace Vitask.Controllers
{

    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInService;

        private readonly UserManager<AppUser> _userService;

        private readonly IAppUserService _appUserService;

        private readonly IMapper _mapper;

		public LoginController(SignInManager<AppUser> signInService, UserManager<AppUser> userService, IAppUserService appUserService, IMapper mapper)
		{
			_signInService = signInService;
			_userService = userService;
			_appUserService = appUserService;
			_mapper = mapper;
		}

		[AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {


            var result = await _signInService.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false);

            if (result.Succeeded)
            {


                return RedirectToAction("Index", "Dashboard");

            }

            return View(loginViewModel);
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {

            AppUser user = _mapper.Map<AppUser>(signUpViewModel);

            var result = await _userService.CreateAsync(user, signUpViewModel.Password);

            

            if (result.Succeeded)
            {
				await _userService.AddToRoleAsync(user, "User");
				return RedirectToAction("Index", "Login");
            }
            else
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View(signUpViewModel);
        }



        [Authorize]
        public async Task<IActionResult> Logout()
        {

            CacheManager.ClearAll();
            await _signInService.SignOutAsync();
            return RedirectToAction("Index", "Login");

        }







    }
}
