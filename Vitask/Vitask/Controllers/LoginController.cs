using System.Security.Claims;
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

		public LoginController(SignInManager<AppUser> signInService, UserManager<AppUser> userService, IAppUserService appUserService)
		{
			_signInService = signInService;
			_userService = userService;
			_appUserService = appUserService;
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

                var user = await _userService.GetUserAsync(User);

                Claim claim = new Claim("Image",user.Image);
                await _userService.AddClaimAsync(user,claim);

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

            AppUser user = new AppUser()
            {
                Name = signUpViewModel.Name,
                Surname = signUpViewModel.Surname,
                Email = signUpViewModel.Email,
                UserName = signUpViewModel.Name + signUpViewModel.Surname

            };

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
