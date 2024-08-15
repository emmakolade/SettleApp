using System.ComponentModel.DataAnnotations;
using Settle_App.Helpers;

namespace Settle_App.Models.DTO
{
    public class PaymentVerificationRequestDto
    {
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public Guid SystemTransactionReference { get; set; }
        
        [EnumDataType(typeof(PaymentGateway))]
        public PaymentGateway PaymentGateway { get; set; } = PaymentGateway.None;

    }



}
