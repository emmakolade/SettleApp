using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Settle_App.Models.DTO.FlutterWave
{
    public class BillsInfoValidateDto
    {
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }


        [JsonPropertyName("biller_code")]
        public string Address { get; set; }


        [JsonPropertyName("response_message")]
        public string ResponseMessage { get; set; }


        [JsonPropertyName("name")]
        public string Name { get; set; }


        [JsonPropertyName("biller_code")]
        public bool? BillerCode { get; set; }


        [JsonPropertyName("customer")]
        public string Customer { get; set; }


        [JsonPropertyName("product_code")]
        public string ProductCode { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }


        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }


        [JsonPropertyName("maximum")]
        public decimal Maximum { get; set; }


        [JsonPropertyName("minimum")]
        public decimal Minimum { get; set; }
    }
    public class BillsInfoValidateResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
        public BillsInfoValidateDto Data { get; set; }

    }

}