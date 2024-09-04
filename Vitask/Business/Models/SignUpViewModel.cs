using System.ComponentModel.DataAnnotations;

namespace Vitask.Models
{
    public class SignUpViewModel
    {

        [Required(ErrorMessage = "Name can not be empty!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname can not be empty!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email can not be empty!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can not be empty!")]
        public string Password { get; set; }
    }
}
