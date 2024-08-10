using System.Runtime.CompilerServices;

namespace Settle_App.Helpers
{
    public class EndpointResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }

    }

    public class InterswitchPaymentResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public decimal Amount { get; set; }
        public string CustomerEmail { get; set; }
        public string PaymentUrl { get; set; }
        public string TransactionReference { get; set; }
    }


    public class InterswitchReqDTO
    {
        public string AccessToken { get; set; }

        // If you need to extract additional fields from the response, add them here
        public string TokenType { get; set; }
        public long? ExpiresIn { get; set; }
       
    }

    
}
