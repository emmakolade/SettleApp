using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Settle_App.Models.DTO.FlutterWave
{
    public class CategoryDto
    {
        [JsonPropertyName("id")]
        public int CategoryId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }
    }
    public class BillsCategoryResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
        public List<CategoryDto> Data { get; set; }

    }

}