
using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete
{
    public class AppRole : IdentityRole<int>
    {

        public int DeletionStateCode { get; set; }
    }
}
