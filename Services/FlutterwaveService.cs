using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using RestSharp;
using Settle_App.Helpers;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Settle_App.Models.DTO.FlutterWave;
using Azure.Core;
using System.Net.Http.Json;

namespace Settle_App.Services
{


    public class FlutterWaveService(IConfiguration configuration)
    {
        private readonly IConfiguration configuration = configuration;

        public async Task<FlutterwavePaymentInitializationResponseDto> InitializePaymentAsync(string amount, string customerEmail, Guid transactionReference)
        {
            try
            {
                var options = new RestClientOptions("https://api.flutterwave.com/v3/payments");
                Console.WriteLine($"options======>>>>>{options}");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("Authorization", $"Bearer FLWSECK_TEST-750ef95bb6081c01e8b2290660911c0f-X");
                request.AddHeader("Content-Type", "application/json");
                var PaybillRequestDto = new FlutterwavePaybillRequestDto
                {
                    TransactionReference = transactionReference,
                    Amount = amount,
                    RedirectUrl = "https://bing.com",
                    CurrencyCode = "NGN",
                    Customer = new FlutterwaveCustomerDto
                    {
                        Email = customerEmail,
                        // Name = "emmakolade",
                        // PhoneNumber = "08144003440"
                    },
                    Customizations = new FlutterwaveCustomizationsDto { Title = "Settle App Wallet Top-Up" },
                    Configurations = new FlutterwaveConfigurationsDto { MaxRetryAttempt = 5, SessionDuration = 10 }
                };
                request.AddJsonBody(PaybillRequestDto);
                Console.WriteLine($"request======>>>>>{request}");
                var response = await client.PostAsync(request);
                Console.WriteLine($"response======>>>>>{response.Content}");
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"payment could not be initialized: ");
                }
                var _json = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var paymentResponse = new FlutterwavePaymentInitializationResponseDto
                {

                    PaymentUrl = _json.GetProperty("data").GetProperty("link").GetString(),

                };
                return paymentResponse;

            }
            catch (Exception Err)
            {

                throw new Exception($"payment initialization failed: {Err.Message}");
            }


        }

        //verify that payment was successful
        public async Task<FlutterwavePaymentVerificationResponseDto> VerifyPaymentAsync(string transactionReference, decimal amount)
        {
            try
            // MX6072
            {
                // LIVE BASE URL: https://webpay.interswitchng.com
                var options = new RestClientOptions($"https://api.flutterwave.com/v3/transactions/?transaction_id={transactionReference}/verify");
                var client = new RestClient(options);

                var request = new RestRequest("");

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer FLWSECK_TEST-750ef95bb6081c01e8b2290660911c0f-X");
                var response = await client.GetAsync(request);
                Console.WriteLine($"response====>{response.Content}");

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var verificationResponse = JsonSerializer.Deserialize<FlutterwavePaymentVerificationResponseDto>(response.Content);

                if (verificationResponse.Amount == amount && verificationResponse.CurrencyCode == "NGN" && verificationResponse.Status == "successful")
                {
                    return verificationResponse;

                }
                else
                {
                    return new FlutterwavePaymentVerificationResponseDto { Status = "payment verification not successful" };
                }

            }
            catch (Exception err)
            {

                throw new Exception($"{err.Message}");
            }

        }
        public async Task<BillsCategoryResponseDto> FetchBillsCategory()
        {
            try
            {
                var options = new RestClientOptions($"https://api.flutterwave.com/v3/top-bill-categories?country=NG");
                var client = new RestClient(options);

                var request = new RestRequest("");

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer FLWSECK_TEST-750ef95bb6081c01e8b2290660911c0f-X");
                var response = await client.GetAsync(request);
                Console.WriteLine($"response====>{response.Content}");

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var billsCategory = JsonSerializer.Deserialize<BillsCategoryResponseDto>(response.Content);

                if (billsCategory.Status == "success")
                {
                    return billsCategory;

                }
                else
                {
                    return new BillsCategoryResponseDto { Status = "Could not get bills category" };
                }

            }
            catch (Exception err)
            {

                throw new Exception($"{err.Message}");
            }

        }
        public async Task<BillersInfoResponseDto> FetchBillersInfo(string billerCode)
        {
            try
            {
                var options = new RestClientOptions($"https://api.flutterwave.com/v3/bills/category={billerCode}/billers?country=NG");
                var client = new RestClient(options);

                var request = new RestRequest("");

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer FLWSECK_TEST-750ef95bb6081c01e8b2290660911c0f-X");
                var response = await client.GetAsync(request);
                Console.WriteLine($"response====>{response.Content}");

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var verificationResponse = JsonSerializer.Deserialize<BillersInfoResponseDto>(response.Content);

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var billerInfo = JsonSerializer.Deserialize<BillersInfoResponseDto>(response.Content);

                if (billerInfo.Status == "success")
                {
                    return billerInfo;

                }
                else
                {
                    return new BillersInfoResponseDto { Status = "Could not get billers Info" };
                }

            }
            catch (Exception err)
            {

                throw new Exception($"{err.Message}");
            }

        }

        public async Task<BilsInfoResponseDto> FetchBillsInfo(string billerCode)
        {
            try
            {
                var options = new RestClientOptions($"https://api.flutterwave.com/v3/billers/biller_code={billerCode}/items");
                var client = new RestClient(options);

                var request = new RestRequest("");

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer FLWSECK_TEST-750ef95bb6081c01e8b2290660911c0f-X");
                var response = await client.GetAsync(request);
                Console.WriteLine($"response====>{response.Content}");

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var verificationResponse = JsonSerializer.Deserialize<BilsInfoResponseDto>(response.Content);

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var billerInfo = JsonSerializer.Deserialize<BilsInfoResponseDto>(response.Content);

                if (billerInfo.Status == "success")
                {
                    return billerInfo;

                }
                else
                {
                    return new BilsInfoResponseDto { Status = "Could not get bills Info" };
                }

            }
            catch (Exception err)
            {

                throw new Exception($"{err.Message}");
            }

        }
        public async Task<BillsInfoValidateResponseDto> ValidateBillsInfo(string itemCode, string billerCode, string customer)
        {
            // customer is either the phone number or the smart card number or for wifi it is the customer account number
            // for electricity, the customer is the meter number ...
            try
            {
                var options = new RestClientOptions($"https://api.flutterwave.com/v3/bill-items/item_code={itemCode}/validate?code={billerCode}&customer={customer}");
                var client = new RestClient(options);

                var request = new RestRequest("");

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer FLWSECK_TEST-750ef95bb6081c01e8b2290660911c0f-X");
                var response = await client.GetAsync(request);
                Console.WriteLine($"response====>{response.Content}");

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var verificationResponse = JsonSerializer.Deserialize<BillsInfoValidateResponseDto>(response.Content);

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var billerInfo = JsonSerializer.Deserialize<BillsInfoValidateResponseDto>(response.Content);

                if (billerInfo.Status == "success")
                {
                    return billerInfo;

                }
                else
                {
                    return new BillsInfoValidateResponseDto { Status = "Could not get billers Info" };
                }

            }
            catch (Exception err)
            {

                throw new Exception($"{err.Message}");
            }

        }

        public async Task<BillsInfoValidateResponseDto> CreateBillsPayment(string itemCode, string billerCode, string customer, decimal amount)
        {
            // customer is either the phone number or the smart card number or for wifi it is the customer account number
            // for electricity, the customer is the meter number ...
            try
            {
                var options = new RestClientOptions($"https://api.flutterwave.com/v3/billers/biller_code={billerCode}/items/item_code={itemCode}/payment");
                var client = new RestClient(options);

                var request = new RestRequest("");

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer FLWSECK_TEST-750ef95bb6081c01e8b2290660911c0f-X");

                var BillsInfoRequest = new BillsInfoRequestDto
                {
                    Country = "NG",
                    Amount = amount,
                    RedirectUrl = "https://bing.com",
                    CustomerId = customer,
                    Reference = Guid.NewGuid()
                };
                request.AddJsonBody(BillsInfoRequest);

                var response = await client.PostAsync(request);
                Console.WriteLine($"response====>{response.Content}");

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var verificationResponse = JsonSerializer.Deserialize<BillsInfoValidateResponseDto>(response.Content);

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var billerInfo = JsonSerializer.Deserialize<BillsInfoValidateResponseDto>(response.Content);

                if (billerInfo.Status == "success")
                {
                    return billerInfo;

                }
                else
                {
                    return new BillsInfoValidateResponseDto { Status = "Could not get billers Info" };
                }

            }
            catch (Exception err)
            {

                throw new Exception($"{err.Message}");
            }

        }

    }
}

