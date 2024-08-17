using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO.FlutterWave
{
    public class FlutterwavePaymentVerificationResponseDto
    {
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string CurrencyCode { get; set; }
    }
}
