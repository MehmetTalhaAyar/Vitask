using System.Security.Claims;
using Business.Abstract;
using Business.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vitask.Controllers
{
	public class FileController : Controller
	{
		private readonly IFileService _fileService;

		private readonly IAppUserService _userService;

		public FileController(IFileService fileService, IAppUserService userService)
		{
			_fileService = fileService;
			_userService = userService;
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> ChangeProfileImage(EditProfileImageModel editProfileImageModel)
		{

			int? userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			if (userId == null)
				return RedirectToAction("Index","Login");

			AppUser user = _userService.GetById((int)userId); ;



			string? image = await _fileService.UploadImageAsync(editProfileImageModel.Image);

			if(image != null)
			{
				if(user.Image !=null)
					await _fileService.DeleteImageAsync(user.Image);


				user.Image = image;

				_userService.Update(user);

			}




			return RedirectToAction($"{user.UserName}","Profile");
		}
	}
}
