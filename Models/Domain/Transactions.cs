using Microsoft.AspNetCore.Identity;
using Settle_App.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Settle_App.Models.Domain
{
  public class Transaction
  {

    [Key]
    public Guid TransactionId { get; set; }
    public string OwnerId { get; set; }
    public decimal Amount { get; set; } = 0;
    public DateTime TransactionTime { get; set; }


    [EnumDataType(typeof(TransactionStatus))]
    public TransactionStatus TransactionStatus { get; set; } = TransactionStatus.Pending;

    
    [EnumDataType(typeof(TransactionType))]
    public TransactionType TransactionType { get; set; } = TransactionType.WalletFunding;
    
    
    [EnumDataType(typeof(PaymentGateway))]
    public PaymentGateway PaymentGateway { get; set; } = PaymentGateway.None;
    
  }
}
