using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Settle_App.Models.DTO.FlutterWave
{
    public class BillersInfoDto
    {
        [JsonPropertyName("id")]
        public int BillersInfoId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("logo")]
        public string Logo { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("biller_code")]
        public string BillerCode { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }
    }
    public class BillersInfoResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
        public List<BillersInfoDto> Data { get; set; }

    }

}