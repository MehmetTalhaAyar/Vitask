using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Vitask.ViewComponents
{
    public class _UserImagePartial : ViewComponent
    {
        private readonly UserManager<AppUser> _userService;

        public _UserImagePartial(UserManager<AppUser> userService)
        {
            _userService = userService;
        }
        
        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var user = await _userService.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);

            ViewData["Image"] = user.Image;
            
            return View("Default", user.Image);
        }
    }
}
