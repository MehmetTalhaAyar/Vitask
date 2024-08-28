

using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete
{
    public class AppUser : IdentityUser<int>
    {

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedOn { get; set; }

        public int DeletionStateCode { get; set; }

        public virtual ICollection<Task> ReporterTasks { get; set; }
        public virtual ICollection<Task> ResponsibleTasks { get; set; }
    }
}
