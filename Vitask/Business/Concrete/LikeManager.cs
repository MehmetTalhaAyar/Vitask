using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using DataAccessLayer.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
	public class LikeManager : ILikeService
	{
		private readonly ILikeDal _likeDal;

		public LikeManager(ILikeDal likeDal)
		{
			_likeDal = likeDal;
		}

		public void Delete(Like t)
		{
			_likeDal.Delete(t);
		}

		public List<Like> GetAll()
		{
			return _likeDal.GetAll();
		}

		public bool DeleteByCommentIdAndUserId(int CommentId, int UserId)
		{
			return _likeDal.DeleteByCommentIdAndUserId(CommentId, UserId);
		}

		public Like GetById(int id)
		{
			return _likeDal.GetById(id);
		}

		public Like Insert(Like t)
		{
			return _likeDal.Insert(t);
		}

		public void Update(Like t)
		{
			_likeDal.Update(t);
		}
	}
}
