﻿using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
    public class InterswitchPaymentInitializationRequestDto
    {
        public string Amount { get; set; }
        // public string RedirectUrl { get; set; }
        public string CustomerEmail { get; set; }

    }



}
