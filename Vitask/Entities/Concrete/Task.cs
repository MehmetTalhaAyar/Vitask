

namespace Entities.Concrete
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } // 0 bitmedi 1 bitti

        public int DeletionStateCode { get; set; }
        public int Priority { get; set; } // 1-9 arasında değer alacak
        public DateTime DueDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime? UpdatedOn { get; set; }

        public int ResponsibleId { get; set; }

        public int ReporterId { get; set; }

        public int ProjectId { get; set; }

        public int TagId { get; set; }


        public virtual AppUser Responsible { get; set; }

        public virtual AppUser Reporter { get; set; }

        public virtual Project Project { get; set; }

        public virtual Tag Tag { get; set; }








    }
}
