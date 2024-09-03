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
	}
}
