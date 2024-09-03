using Business.Abstract;
using Business.Concrete;
using Microsoft.AspNetCore.Mvc;
using Vitask.Models;

namespace Vitask.ViewComponents
{
	public class _ProjectUsersSelectPartial : ViewComponent
	{
		private readonly IAppUserService _appUserService;

		public _ProjectUsersSelectPartial(IAppUserService appUserService)
		{
			_appUserService = appUserService;
		}

		public IViewComponentResult Invoke(string keyword)
		{

			var values = SelectList(keyword);



			return View(values);
		}


		public List<SelectListItemViewModel> SelectList(string keyword)
		{
			return _appUserService.GetUsersByKeyword(keyword).Select(x=> new SelectListItemViewModel()
			{
				id = x.Id,
				text = x.UserName
			}).ToList();


		}
	}
}
