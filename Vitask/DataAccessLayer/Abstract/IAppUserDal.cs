using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace DataAccessLayer.Abstract
{
	public interface IAppUserDal
	{

		List<AppUser> GetUsersByKeyword(string keyword);

		List<AppUser> GetAllUsers();

		AppUser GetById(int id);

	}
}
