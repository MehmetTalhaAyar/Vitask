using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
	public class UserInfo
	{
		public int Id { get; set;}

		public string? Title { get; set;}

		public string? About { get; set;}

		public string? Location { get; set;}

		public int UserId { get; set;}

		public virtual AppUser User { get; set;}


	}
}
