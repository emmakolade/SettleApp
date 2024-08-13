using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
    public class InterswitchPaymentInitializationResponseDto
    {
       
        public string TransactionReference { get; set; }
        public string Amount { get; set; }
        public string RedirectUrl { get; set; }
        public string PaymentUrl { get; set; }
        public string CustomerId { get; set; }
        public string PayableCode { get; set; }
        // Add other fields if necessary
    }

}
