using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccessLayer.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
	public class AppUserManager : IAppUserService
	{
		private readonly IAppUserDal _appUserDal;

		public AppUserManager(IAppUserDal appUserDal)
		{
			_appUserDal = appUserDal;
		}

		public List<AppUser> GetAllUsers()
		{
			return _appUserDal.GetAllUsers();
		}

		public List<AppUser> GetUsersByKeyword(string keyword)
		{
			return _appUserDal.GetUsersByKeyword(keyword);
		}
	}
}
