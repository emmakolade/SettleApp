namespace Settle_App.Helpers
{
    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed,
        Cancelled
    }
    public enum TransactionType
    {
        WalletFunding,
        AirtimeTopUp,
        DataTopUp,
        TvSubscription
    }
    public enum PaymentGateway
    {
        None,
        Interswitch,
        FlutterWave,
        Paystack
    }
}
