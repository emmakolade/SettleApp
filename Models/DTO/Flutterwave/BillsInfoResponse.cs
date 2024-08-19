using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Settle_App.Models.DTO.FlutterWave
{
    public class BillsInfoDto
    {
        [JsonPropertyName("id")]
        public int BillersInfoId { get; set; }

        [JsonPropertyName("biller_code")]
        public string BillerCode { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_airtime")]
        public bool? IsAirtime { get; set; } = false;

        [JsonPropertyName("item_code")]
        public string ItemCode { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }
    }
    public class BilsInfoResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
        public List<BillsInfoDto> Data { get; set; }

    }

}