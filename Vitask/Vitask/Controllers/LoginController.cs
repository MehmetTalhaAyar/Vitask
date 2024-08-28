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


        public LoginController(SignInManager<AppUser> signInService)
        {
            _signInService = signInService;
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




            return View();
        }







    }
}
