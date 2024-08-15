using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Settle_App.Models.DTO
{
    public class PaystackPaybillRequestDto
    {
        [JsonPropertyName("reference")]
        public Guid TransactionReference { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("email")]
        public string CustomerEmail { get; set; }

        [JsonPropertyName("callback_url")]
        public string RedirectUrl { get; set; }
        
        [JsonPropertyName("currency")]
        public string CurrencyCode { get; set; }

    }

}
