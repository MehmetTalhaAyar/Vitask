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
	public class CommentManager : ICommentService
	{
		private readonly ICommentDal _commentDal;

		public CommentManager(ICommentDal commentDal)
		{
			_commentDal = commentDal;
		}

		public void Delete(Comment t)
		{
			_commentDal.Delete(t);
		}

		public List<Comment> GetAll()
		{
			return _commentDal.GetAll();
		}

		public List<Comment> GetAllByTaskId(int taskId)
		{
			return _commentDal.GetAllByTaskId(taskId);
		}

		public List<Comment>? GetAllReplys(int CommentId)
		{
			return _commentDal.GetAllReplys(CommentId);
		}

		public Comment GetById(int id)
		{
			return _commentDal.GetById(id);
		}

		public Comment Insert(Comment t)
		{
			return _commentDal.Insert(t);
		}

		public void Update(Comment t)
		{
			_commentDal.Update(t);
		}
	}
}
