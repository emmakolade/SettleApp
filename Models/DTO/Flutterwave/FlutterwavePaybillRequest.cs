using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Settle_App.Models.DTO.FlutterWave
{
    public class FlutterwavePaybillRequestDto
    {
        [JsonPropertyName("tx_ref")]
        public Guid TransactionReference { get; set; }
        
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonPropertyName("currency")]
        public string CurrencyCode { get; set; }
        public FlutterwaveCustomerDto? Customer { get; set; }
        public FlutterwaveCustomizationsDto? Customizations { get; set; }
        public FlutterwaveConfigurationsDto? Configurations { get; set; }

    }
    public class FlutterwaveCustomerDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; }
    }

    public class FlutterwaveCustomizationsDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }


    public class FlutterwaveConfigurationsDto
    {
        [JsonPropertyName("session_duration")]
        public int SessionDuration { get; set; } = 10;
        
        [JsonPropertyName("max_retry_attempt")]
        public int MaxRetryAttempt { get; set; } = 5;
    }



}
