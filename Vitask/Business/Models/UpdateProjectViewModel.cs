namespace Vitask.Models
{
	public class UpdateProjectViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string Description { get; set; }

		public int CommanderId { get; set; }

		public List<int> UserIds { get; set; }
	}
}
