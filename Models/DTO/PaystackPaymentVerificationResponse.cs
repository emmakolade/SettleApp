using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
     public class PaystackPaymentVerificationResponseDto
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime PaymentDate { get; set; }

    }

}
