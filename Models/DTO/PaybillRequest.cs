using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
    public class PaybillRequestDto
    {
        public string MerchantCode { get; set; }
        public string PayableCode { get; set; }
        public string Amount { get; set; }
        public string RedirectUrl { get; set; }
        public string CustomerId { get; set; }
        public string CurrencyCode { get; set; }
        public string CustomerEmail { get; set; }
    }



}
