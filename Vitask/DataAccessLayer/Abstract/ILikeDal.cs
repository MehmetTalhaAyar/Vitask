using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace DataAccessLayer.Abstract
{
	public interface ILikeDal : IGenericDal<Like>
	{
		bool DeleteByCommentIdAndUserId(int CommentId, int UserId);
	}
}
