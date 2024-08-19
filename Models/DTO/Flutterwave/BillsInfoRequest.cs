using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Settle_App.Models.DTO.FlutterWave
{
    public class BillsInfoRequestDto
    {

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("reference")]
        public Guid Reference { get; set; }

        [JsonPropertyName("callback_url")]
        public string RedirectUrl { get; set; }


    }




}
