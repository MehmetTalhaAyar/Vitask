using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
	public class CommentDal : GenericRepository<Comment>, ICommentDal
	{
		public List<Comment> GetAllByTaskId(int taskId)
		{
			using (VitaskContext context = new VitaskContext())
			{
				return context.Comments
					.Include(x => x.User)
					.Include(x => x.Likes)
					.Include(x => x.Replys)
					.Where(x => x.TaskId == taskId && x.ParentCommentId == null).OrderByDescending(x=>x.CreatedOn).ToList();
			}
		}

		public List<Comment>? GetAllReplys(int CommentId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				var relativeComment = context.Comments
					.Include(x => x.Replys)
					.ThenInclude(x => x.User )
					.Include(x => x.Replys)
					.ThenInclude(x => x.Likes)
					.Where(x => x.Id == CommentId).FirstOrDefault();

				if (relativeComment != null)
				{
					return relativeComment.Replys.OrderByDescending(x => x.CreatedOn).ToList();
				}
				else
				{
					return null;
				}
			}
		}
	}
}
