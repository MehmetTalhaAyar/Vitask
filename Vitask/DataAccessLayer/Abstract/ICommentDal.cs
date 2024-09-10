using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace DataAccessLayer.Abstract
{
	public interface ICommentDal : IGenericDal<Comment>
	{
		List<Comment> GetAllByTaskId(int taskId);

		List<Comment>? GetAllReplys(int CommentId);

	}
}
