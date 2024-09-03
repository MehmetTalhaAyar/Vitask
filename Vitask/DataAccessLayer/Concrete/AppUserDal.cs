using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Entities.Concrete;

namespace DataAccessLayer.Concrete
{
	public class AppUserDal : IAppUserDal
	{
		public List<AppUser> GetAllUsers()
		{
			using (VitaskContext context = new VitaskContext())
			{
				return context.Users.ToList();
			}
		}

		public AppUser GetById(int id)
		{
			using(VitaskContext context = new VitaskContext())
			{
				return context.Users.Where(x => x.Id == id).FirstOrDefault();
			}
		}

		public List<AppUser> GetUsersByKeyword(string keyword)
		{

			var value = keyword != null ? keyword : "";
			using (VitaskContext context = new VitaskContext())
			{
				return context.Users.Where(x => x.UserName.ToLower().Contains(value.ToLower())).Take(5).ToList();
			}
		}
	}
}
