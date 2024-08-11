using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Settle_App.Data;
using Settle_App.Helpers;
using Settle_App.Models.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Settle_App.Repositories
{
    public class WalletRepository(SettleAppDBContext dbContext) : IWalletRepository
    {
        private readonly SettleAppDBContext dbContext = dbContext;

        public async Task<Wallet> GetWalletByIdAsync(string OwnerId)
        {
            return await dbContext.Wallet.SingleOrDefaultAsync(w => w.OwnerId == OwnerId);

        }
        public async Task UpdateWalletBalanceAsync(Wallet wallet, decimal amount)
        {
            wallet.Balance += amount / 100;
            wallet.LastUpdated = DateTime.UtcNow;
            dbContext.Wallet.Update(wallet);
            await dbContext.SaveChangesAsync();            
        }

        public async Task<Wallet> CreateWalletAsync(SettleAppUser settleAppUser){
            var wallet = new Wallet{
                WalletId = Guid.NewGuid(),
                OwnerId = settleAppUser.Id,
                Balance = 0,
                LastUpdated = DateTime.UtcNow
            };
            dbContext.Wallet.Add(wallet);
            await dbContext.SaveChangesAsync();
            return wallet;
        }
    }

}
