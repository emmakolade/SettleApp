using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
     public class PaymentVerificationResponseDto
    {
        public string TransactionReference { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string MerchantCode { get; set; }
        public string PaymentDate { get; set; }
        // Add other fields if necessary
    }



}
