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
		public List<Task> GetAllByProjectId(int ProjectId, int page)
		{
			using(VitaskContext context = new VitaskContext())
			{
				return context.Tasks
					.Include(x => x.Responsible)
					.Include(x => x.Reporter)
					.Include(x => x.Tag)
					.OrderBy(x=>x.DueDate)
					.Where(x=> x.ProjectId == ProjectId).Skip((page-1) * 10).Take(10).ToList();
			}
		}

		public List<Task> GetAllByUserId(int UserId)
		{
			using (VitaskContext context = new VitaskContext())
			{
				return context.Tasks.Where(x => x.ResponsibleId == UserId).ToList();
			}
		}

		public int GetPageCount(int ProjectId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				double pageCount = ((double)context.Tasks.Where(x => x.ProjectId == ProjectId).Count() / 10);
				return (int)Math.Ceiling(pageCount);
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
