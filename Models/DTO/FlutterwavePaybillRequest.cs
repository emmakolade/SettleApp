using System.ComponentModel.DataAnnotations;

namespace Settle_App.Models.DTO
{
    public class FlutterwavePaybillRequestDto
    {
        public Guid TransactionReference { get; set; }
        public string Amount { get; set; }
        public string RedirectUrl { get; set; }
        public string CurrencyCode { get; set; }
        public FlutterwaveCustomerDto? Customer { get; set; }
        public FlutterwaveCustomizationsDto? Customizations { get; set; }
        public FlutterwaveConfigurationsDto? Configurations { get; set; }

    }
    public class FlutterwaveCustomerDto
    {
        public string Email { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class FlutterwaveCustomizationsDto
    {
        public string Title { get; set; }
    }


    public class FlutterwaveConfigurationsDto
    {
        public int SessionDuration { get; set; } = 10;
        public int MaxRetryAttempt { get; set; } = 5;
    }



}
