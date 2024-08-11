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
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string Scope { get; set; }
        public string MerchantCode { get; set; }
        public string ClientName { get; set; }
        public string PayableId { get; set; }
        public string Jti { get; set; }

    }


}
