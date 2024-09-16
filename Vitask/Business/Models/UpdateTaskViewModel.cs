using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
	public class UpdateTaskViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
		public DateTime DueTime { get; set; }
		public int ResponsibleId { get; set; }
		public int ReporterId { get; set; }
		public int TagId { get; set; }




	}
}
