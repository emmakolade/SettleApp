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
    public class TransactionRepository(SettleAppDBContext dbContext) : ITransactionRepository
    {
        private readonly SettleAppDBContext dbContext = dbContext;

        public async Task<Transaction> CreateTransactionAsync(SettleAppUser settleAppUser, decimal amount, Guid transactionReference, TransactionStatus transactionStatus, TransactionType transactionType, PaymentGateway paymentGateway)
        {
            var transaction = new Transaction
            {
                TransactionId = transactionReference,
                OwnerId = settleAppUser.Id,
                Amount = amount,
                PaymentGateway = paymentGateway,
                TransactionStatus = transactionStatus,
                TransactionType = transactionType,
                TransactionTime = DateTime.UtcNow,

            };
            dbContext.Transactions.Add(transaction);
            await dbContext.SaveChangesAsync();
            return transaction;
        }
        public async Task UpdateTransactionAsync(Transaction transaction, decimal amount, TransactionStatus transactionStatus, TransactionType transactionType, PaymentGateway paymentGateway, DateTime TransactionTime)


        {

            transaction.Amount = amount;
            transaction.TransactionStatus = transactionStatus;
            transaction.TransactionType = transactionType;
            transaction.PaymentGateway = paymentGateway;
            transaction.TransactionTime = DateTime.UtcNow;
            dbContext.Transactions.Update(transaction);
            await dbContext.SaveChangesAsync();

        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid TransactionId)
        {
            return await dbContext.Transactions.SingleOrDefaultAsync(t => t.TransactionId == TransactionId);

        }




    }

}
