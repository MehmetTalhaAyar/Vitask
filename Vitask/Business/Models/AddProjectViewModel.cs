namespace Vitask.Models
{
	public class AddProjectViewModel
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public int CommanderId { get; set; }

		public List<int> UserIds { get; set; }
	}
}
