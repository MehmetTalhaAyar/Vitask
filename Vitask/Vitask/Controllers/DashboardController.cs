using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Vitask.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ITaskService _taskService;

        private readonly UserManager<AppUser> _userService;

		public DashboardController(ITaskService taskService, UserManager<AppUser> userService)
		{
			_taskService = taskService;
			_userService = userService;
		}

        [Authorize]
		public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserAsync(User);

            if(user == null)
            {
                return RedirectToAction("Login","Index");
            }


            int TaskCount = _taskService.GetTaskCountForUser(user.Id); // burada aynı zamanda due task count ta gelmesi gerekiyor
            



            return View();
        }
    }
}
