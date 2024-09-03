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
	public class ProjectUserDal : GenericRepository<ProjectUser>, IProjectUserDal
	{
		public void CreateProjectUserList(List<int> Ids, int ProjectId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				foreach(var id in Ids)
				{
					ProjectUser user = new ProjectUser()
					{
						ProjectId = ProjectId,
						UserId = id
					};

					context.Add(user);
					
					
				}

				context.SaveChanges();
			}
		}

		public List<int> GetUserIdByProject(int ProjectId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				return context.ProjectUsers.Where(x=> x.ProjectId == ProjectId).Select(x=>x.UserId).ToList();
			}
		}

		public void UpdateProjectUserList(List<int> Ids, int ProjectId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				var deletedProjectUsers = context.ProjectUsers.Where(x=> x.ProjectId == ProjectId && !Ids.Contains(x.UserId)).ToList();

				var users = context.ProjectUsers.Where(x=> x.ProjectId == ProjectId && Ids.Contains(x.UserId)).Select(x=>x.UserId).ToList();

				foreach(var userId in Ids)
				{
					if (!users.Contains(userId))
					{
						ProjectUser projectUser = new ProjectUser()
						{
							ProjectId = ProjectId,
							UserId = userId
						};

						context.Add(projectUser);
						context.SaveChanges();

					}
				}

				foreach(var projectUser in deletedProjectUsers)
				{
					context.Remove(projectUser);
					context.SaveChanges();
				}

				
			}
		}
	}
}
