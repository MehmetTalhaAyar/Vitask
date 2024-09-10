

namespace Entities.Concrete
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public int DeletionStateCode { get; set; } = 0;
        public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();

        public int TaskId { get; set; }

        public int UserId { get; set; } // yorumu yapan kişi
    
        public int? ParentCommentId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Task Task { get; set; }

        public virtual Comment? ParentComment { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Replys { get; set; }






    }
}
