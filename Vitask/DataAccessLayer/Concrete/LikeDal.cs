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
	public class LikeDal : GenericRepository<Like>, ILikeDal
	{
		public bool DeleteByCommentIdAndUserId(int CommentId, int UserId)
		{
			using(VitaskContext context = new VitaskContext())
			{
				var like = context.Likes.Where(x => x.UserId == UserId && x.CommentId == CommentId).FirstOrDefault();
				if(like != null)
				{
					context.Remove(like);
					context.SaveChanges();
					return true;
				}

				return false;

			}
		}
	}
}
