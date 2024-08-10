using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using Settle_App.Models.Domain;
using System.Security.Claims;


namespace Settle_App.Repositories
{
    public interface IWalletRepository
    {

        Task<Wallet> GetWalletByIdAsync(string OwnerId);
        Task<Wallet> CreateWalletAsync(SettleAppUser settleAppUser);
        Task UpdateWalletBalanceAsync(Wallet wallet, decimal amount);
        
    }

  
}
