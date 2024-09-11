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
		public List<AppUser> GetAllUsers(int Page, int? exceptId = null)
		{
			using (VitaskContext context = new VitaskContext())
			{
				if(exceptId == null)
				{
					return context.Users.Skip((Page - 1) * 10).Take(10)
						.ToList();
				}
				else
				{
					return context.Users.Where(x=> x.Id != exceptId)
						.Skip((Page - 1) * 10).Take(10).ToList();
				}
			}
		}

		public int GetPageCount(int? exceptId = null)
		{
			using(VitaskContext context = new VitaskContext())
			{
				if(exceptId == null)
				{
					double pageCount = (double)context.Users.Count() / 10;
					return (int)Math.Ceiling(pageCount);
				}
				else
				{
					double pageCount = (double)context.Users.Where(x=>x.Id != exceptId).Count() / 10;
					return (int)Math.Ceiling(pageCount);
				}
			}
		}

		public AppUser GetById(int id)
		{
			using(VitaskContext context = new VitaskContext())
			{
				return context.Users.Where(x => x.Id == id).FirstOrDefault();
			}
		}

		public List<AppUser> GetUsersByKeyword(string keyword,int? ProjectId)
		{

			var value = keyword != null ? keyword : "";
			using (VitaskContext context = new VitaskContext())
			{
				if(ProjectId == null)
				{
					return context.Users.Where(x => x.UserName.ToLower().Contains(value.ToLower())).Take(5).ToList();
				}
				else
				{
					var ProjectUsers = context.ProjectUsers.Where(x => x.ProjectId == ProjectId).Select(x => x.UserId).ToList();

					return context.Users.Where(x => x.UserName.ToLower().Contains(value.ToLower()) && ProjectUsers.Contains(x.Id)).Take(5).ToList();
				}

				
			}
		}

		public void Delete(int id)
		{
			using(VitaskContext context = new VitaskContext())
			{
				var user = context.Users.Where(x=> x.Id==id).FirstOrDefault();
				user.DeletionStateCode = 1;
				context.Update(user);
				context.SaveChanges();
			}
		}

		public void Update(AppUser appUser)
		{
			using(VitaskContext context = new VitaskContext())
			{

				context.Update(appUser);
				context.SaveChanges();
			}
		}
	}
}
