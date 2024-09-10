﻿using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vitask.Models;

namespace Vitask.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userService;
        public ProfileController(UserManager<AppUser> userService)
        {
            _userService = userService;
        }


        [Authorize]
        [HttpGet("/Profile/{userName}")]
        public async Task<IActionResult> Index([FromRoute] string userName)
        {
            var value = await _userService.FindByNameAsync(userName);
            ProfileViewModel model = new ProfileViewModel()
            {
                Name = value.Name,
                Surname = value.Surname,
                Email = value.Email,
                PictureUrl = value.PhoneNumber,
                Username = value.UserName
            };

            return View(model);
        }


        [Authorize]
        public IActionResult AccountSettings()
        {



            return View();
        }



    }
}
