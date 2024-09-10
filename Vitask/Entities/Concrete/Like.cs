using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Like
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CommentId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();

        public virtual AppUser User { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
