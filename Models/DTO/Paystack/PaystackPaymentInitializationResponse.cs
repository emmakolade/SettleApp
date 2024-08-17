using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO.Paystack
{
    public class PaystackPaymentInitializationResponseDto
    {
        public string PaymentUrl { get; set; }
        public string TransactionReference { get; set; }
    }

}
