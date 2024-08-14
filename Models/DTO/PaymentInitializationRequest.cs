using System.ComponentModel.DataAnnotations;
using Settle_App.Helpers;

namespace Settle_App.Models.DTO
{
    public class PaymentInitializationRequestDto
    {
        public string? Amount { get; set; }
        // public string RedirectUrl { get; set; }
        public string? CustomerEmail { get; set; }
        [EnumDataType(typeof(PaymentGateway))]
        public PaymentGateway PaymentGateway { get; set; } = PaymentGateway.None;

    }



}
