using System.ComponentModel.DataAnnotations;
using Settle_App.Helpers;

namespace Settle_App.Models.DTO
{
    public class InterswitchPaymentVerificationRequestDto
    {
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        
        [EnumDataType(typeof(PaymentGateway))]
        public PaymentGateway PaymentGateway { get; set; } = PaymentGateway.None;

    }



}
