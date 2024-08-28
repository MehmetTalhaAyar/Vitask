

namespace Entities.Concrete
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DeletionStateCode { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }


    }
}
