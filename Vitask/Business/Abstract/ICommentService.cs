using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Models;
using Entities.Concrete;

namespace Business.Abstract
{
	public interface ICommentService : IGenericService<Comment>
	{
		List<Comment> GetAllByTaskId(int taskId);
		List<Comment>? GetAllReplys(int CommentId);
	}
}
