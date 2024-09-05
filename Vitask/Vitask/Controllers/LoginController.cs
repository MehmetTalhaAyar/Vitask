using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vitask.Models;

namespace Vitask.Controllers
{

    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInService;

        private readonly UserManager<AppUser> _userService;

		public LoginController(SignInManager<AppUser> signInService, UserManager<AppUser> userService)
		{
			_signInService = signInService;
			_userService = userService;
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

            await _signInService.SignOutAsync();
            return RedirectToAction("Index", "Login");

        }







    }
}
