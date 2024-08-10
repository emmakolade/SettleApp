using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Settle_App.Models.Domain
{
  public class Wallet
  {
    public Guid WalletId { get; set; }

    // [ForeignKey("SettleAppUser")]
    public string OwnerId { get; set; }
    public decimal Balance { get; set; } = 0;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    // Navigation Properties
    public SettleAppUser WalletOwner { get; set; }
  }
}
