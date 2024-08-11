using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
    public class PaymentVerificationRequestDto
    {
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }

    }



}
