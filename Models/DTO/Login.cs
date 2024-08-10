﻿using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
    public class LoginDto
    {
        

       

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
    }
}
