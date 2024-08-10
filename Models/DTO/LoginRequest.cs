namespace Settle_App.Models.DTO
{
    public class LoginRequestDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
    }
}
