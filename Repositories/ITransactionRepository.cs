using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using Settle_App.Helpers;
using Settle_App.Models.Domain;
using System.Security.Claims;


namespace Settle_App.Repositories
{
    public interface ITransactionRepository
    {

        Task<Transaction> CreateTransactionAsync(SettleAppUser settleAppUser, decimal amount, 
                                                            TransactionStatus transactionStatus, TransactionType transactionType, PaymentGateway paymentGateway);

        Task UpdateTransactionAsync(Transaction transaction, decimal amount, TransactionStatus transactionStatus, TransactionType transactionType, PaymentGateway paymentGateway, DateTime TransactionTime);


        Task<Transaction> GetTransactionByIdAsync(Guid TransactionReference);

    }


}
