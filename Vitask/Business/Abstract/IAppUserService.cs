using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Vitask.Models;

namespace Business.Abstract
{
	public interface IAppUserService
	{
		List<AppUser> GetUsersByKeyword(string keyword, int? ProjectId);

		List<AppUser> GetAllUsers(int Page,int? exceptId = null);

		public int GetPageCount(int? exceptId = null);

		AppUser GetById(int id);

		void Delete(int id);

		public List<SelectListItemViewModel> SelectList(string keyword, int? ProjectId, List<int>? selectedUsers = null);


		public void Update(AppUser appUser);
	}
}
