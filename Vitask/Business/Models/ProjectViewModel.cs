using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitask.Models;

namespace Business.Models
{
	public class ProjectViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string Description { get; set; }

		public UserViewModel Leader { get; set; }

	}
}
