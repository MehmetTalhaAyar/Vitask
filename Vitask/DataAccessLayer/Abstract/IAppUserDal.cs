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

		List<AppUser> GetUsersByKeyword(string keyword,int? ProjectId);

		List<AppUser> GetAllUsers(int Page, int? exceptId = null);

		AppUser GetById(int id);

		public void Delete(int id);

		public int GetPageCount(int? exceptId = null);
	}
}
