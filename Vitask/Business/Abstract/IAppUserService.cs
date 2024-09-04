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
		List<AppUser> GetUsersByKeyword(string keyword);

		List<AppUser> GetAllUsers();

		AppUser GetById(int id);

		void Delete(int id);

		public List<SelectListItemViewModel> SelectList(string keyword, List<int>? selectedUsers = null);

    }
}
