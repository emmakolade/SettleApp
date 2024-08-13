using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Settle_App.Models.Domain
{
    public class SettleAppUser : IdentityUser
    {
        // [Index(IsUnique = true)]
        public string SettleAppUserName { get; set; }
        public string SettleAppFullName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? TransactionReference { get; set; }
    }
}
