using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
    public class RegisterDto
    {
        

        [Required(ErrorMessage = "Full Name is required")]
        public string SettleAppFullName { get; set; }

        [Required(ErrorMessage = "username is required")]
        public string SettleAppUserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confrim Password is required")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; }

       
    }
}
