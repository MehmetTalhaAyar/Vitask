using Business.Models;

namespace Vitask.Models
{
    public class TaskViewModel
    {


        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public UserViewModel ResponsibleId { get; set; } 
        public UserViewModel ReporterId { get; set; } 
        public DateTime CreatedOn { get; set; }

        public DateTime DueTime { get; set; }

        public AllTagsViewModel Tag { get; set; }

        public ProjectViewModel Project { get; set; }

        public List<CommentViewModel> Comments { get; set; }











    }
}
