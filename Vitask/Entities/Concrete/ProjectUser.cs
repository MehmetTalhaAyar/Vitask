using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProjectUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public virtual AppUser User { get; set; }

        public virtual Project Project { get; set; }


    }
}
