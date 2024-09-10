

using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete
{
    public class AppUser : IdentityUser<int>
    {

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();
        
        public DateTime? UpdatedOn { get; set; }

        public int DeletionStateCode { get; set; }

        public string Image { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        public virtual ICollection<Task> ReporterTasks { get; set; }
        public virtual ICollection<Task> ResponsibleTasks { get; set; }

        public virtual ICollection<ProjectUser> Projects { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

    }
}
