namespace Vitask.Models
{
	public class AddTaskViewModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
		public DateTime DueTime { get; set; }
		public int ResponsibleId { get; set; }
		public int ReporterId { get; set; }
		public int TagId { get; set; }

		public int ProjectId { get; set; }
	}
}
