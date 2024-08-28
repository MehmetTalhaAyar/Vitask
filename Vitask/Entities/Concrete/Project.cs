

namespace Entities.Concrete
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? UpdatedOn { get; set; }

        public int DeletionStateCode { get; set; }

        public int CommanderId { get; set; }

        public virtual AppUser User { get; set; }





    }
}
