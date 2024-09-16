using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
					.Where(x=> x.ProjectId == ProjectId)
					.OrderBy(x => x.DueDate)
					.Skip((page-1) * 10).Take(10).ToList();
			}
		}

		public List<Task> GetAllByUserId(int UserId)
		{
			using (VitaskContext context = new VitaskContext())
			{
				return context.Tasks.Where(x => x.ResponsibleId == UserId).ToList();
			}
		}

		public List<Task> GetAllByResponsibleId(int UserId, int page)
		{
			using (VitaskContext context = new VitaskContext())
			{
				return context.Tasks
					.Include(x => x.Responsible)
					.Include(x => x.Reporter)
					.Include(x => x.Tag)
					.Include(x => x.Project)
					.Where(x => x.ResponsibleId == UserId)
					.OrderBy(x => x.DueDate)
					.Skip((page - 1) * 10).Take(10).ToList();
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

		public int GetPageCountByUserId(int UserId)
		{
			using(var context = new VitaskContext())
			{
				double pageCount = ((double)context.Tasks.Where(x => x.ResponsibleId == UserId).Count() / 10);
				return (int) Math.Ceiling(pageCount);
			}
		}


		public int GetTaskCountForUser(int UserId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				return context.Tasks.Where(x => x.ResponsibleId == UserId).Count();
			}
		}

        public Task GetTaskWithRelations(int id)
        {
            using(VitaskContext context = new VitaskContext())
			{
				var tasks =  context.Tasks
					.Include(x => x.Responsible)
					.Include(x => x.Reporter)
					.Include(x => x.Tag)
					.Include(x => x.Project)
					.Include(x=>x.Comments)
					.ThenInclude(x=>x.Replys)
					.ThenInclude(x=>x.Likes)
					.Include(x=>x.Comments)
					.ThenInclude(x=>x.Likes)
					.Include(x=>x.Comments)
					.ThenInclude(x=>x.User)
					.Include(x=>x.Comments)
					.ThenInclude(x=>x.Replys)
					.ThenInclude(x=>x.User)
					.Where(x => x.Id == id).FirstOrDefault();
				var comments = tasks.Comments.Where(x => x.ParentCommentId == null).ToList();
				tasks.Comments = comments;
				return tasks;
            }
        }
    }
}
