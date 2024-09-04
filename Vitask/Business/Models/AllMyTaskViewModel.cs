namespace Vitask.Models
{
	public class AllMyTaskViewModel
	{

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
		public DateTime DueTime { get; set; }
		public string Responsible { get; set; }
		public string Reporter { get; set; }

		public string ProjectName { get; set; }
		public string Tag { get; set; }

	}
}
