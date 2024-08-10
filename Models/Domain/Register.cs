using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.Domain
{
    public class Register : IdentityUser
    {
        public string FullName { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
      //  public bool IsEmaiConfirmed { get; set; } = false;
        //public bool IsPhoneNumberConfirmed { get; set; } = false;


    }
}
