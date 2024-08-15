using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
    public class PaystackPaymentInitializationResponseDto
    {
        public string PaymentUrl { get; set; }
        public string TransactionReference { get; set; }
    }

}
