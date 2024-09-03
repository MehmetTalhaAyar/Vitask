using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Task = Entities.Concrete.Task;

namespace DataAccessLayer.Concrete
{
	public class TaskDal : GenericRepository<Task>, ITaskDal
	{
		public List<Task> GetAllByProjectId(int ProjectId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				return context.Tasks
					.Include(x => x.Responsible)
					.Include(x => x.Reporter)
					.Include(x => x.Tag)
					.OrderBy(x=>x.DueDate)
					.Where(x=> x.ProjectId == ProjectId).ToList();
			}
		}

		public List<Task> GetAllByUserId(int UserId)
		{
			using (VitaskContext context = new VitaskContext())
			{
				return context.Tasks.Where(x => x.ResponsibleId == UserId).ToList();
			}
		}

		public int GetTaskCountForUser(int UserId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				return context.Tasks.Where(x => x.ResponsibleId == UserId).Count();
			}
		}
	}
}
