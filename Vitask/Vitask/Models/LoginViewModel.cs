using System.ComponentModel.DataAnnotations;

namespace Vitask.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email field cannot be empty")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field cannot be empty")]
        public string Password { get; set; }
    }
}
