using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitask.Models;

namespace Business.Models
{
	public class CommentViewModel
	{
		public int Id { get; set; }
		public string Content { get; set; }

		public DateTime CreatedOn { get; set; }

		public UserViewModel User { get; set; }

		public bool isLike { get; set; } = false;

		public List<CommentViewModel>? Replys { get; set; }

	}
}
