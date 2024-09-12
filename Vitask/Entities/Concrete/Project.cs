

namespace Entities.Concrete
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();

        public DateTime? UpdatedOn { get; set; }

        public int DeletionStateCode { get; set; }

        public int CommanderId { get; set; }

        public virtual AppUser Commander { get; set; }

        public virtual ICollection<ProjectUser> Users { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }





    }
}
