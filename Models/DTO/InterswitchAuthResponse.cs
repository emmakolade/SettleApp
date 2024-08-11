using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
    public class InterswitchAuthResponseDto
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
