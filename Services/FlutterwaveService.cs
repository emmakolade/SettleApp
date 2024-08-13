using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using RestSharp;
using Settle_App.Helpers;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Settle_App.Models.DTO;
using Azure.Core;
using System.Net.Http.Json;

namespace Settle_App.Services
{


    public class FlutterWaveService(IConfiguration configuration)
    {
        private readonly IConfiguration configuration = configuration;

        public async Task<FlutterwavePaymentInitializationResponseDto> InitializePaymentAsync(string amount, string customerEmail)
        {
            try
            {
                var options = new RestClientOptions("https://api.flutterwave.com/v3/payments");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("Authorization", $"Bearer {"SECRETKEY"}");
                request.AddHeader("Content-Type", "application/json");
                var PaybillRequestDto = new FlutterwavePaybillRequestDto
                {
                    TransactionReference = Guid.NewGuid(),
                    Amount = amount,
                    RedirectUrl = "https://bing.com",
                    CurrencyCode = "NGN",
                    Customer = new FlutterwaveCustomerDto { Email = customerEmail },
                    Customizations = new FlutterwaveCustomizationsDto { Title = "Settle App Wallet Top-Up" },
                    Configurations = new FlutterwaveConfigurationsDto { MaxRetryAttempt = 5, SessionDuration = 10 }
                };
                request.AddJsonBody(PaybillRequestDto);
                var response = await client.PostAsync(request);
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
                var options = new RestClientOptions($"https://api.flutterwave.com/v3/transactions/?id={transactionReference}/verify");
                var client = new RestClient(options);

                var request = new RestRequest("");

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {"SECRETKEY"}");
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
    }
}

/*

var options = new RestClientOptions("https://qa.interswitchng.com/paymentgateway/api/v1/paybill");
var client = new RestClient(options);
var request = new RestRequest("");
request.AddHeader("accept", "application/json");
request.AddHeader("Authorization", "Bearer eyJhbGciOiJSUzI1NiJ9.eyJhdWQiOlsicGFzc3BvcnQiLCJwcm9qZWN0LXgtbWVyY2hhbnQiXSwic2NvcGUiOlsicHJvZmlsZSJdLCJ2aXJ0dWFsX2FjY291bnRfcHJvdmlkZXIiOiJXRU1BX1BST0QiLCJjbGllbnRfbmFtZSI6IldFTUEgVVNTRCIsImp0aSI6ImZmNWQ2NGIzLWQyZDYtNDc2ZC05NmVlLTUzZTAzYjE4ZmE2YiIsImNsaWVudF9pZCI6IklLSUE3ODlEQkNFRkZGMTIxNkI5NUY1NkFGNDIzNDlERTY2QTlGOUZGQzA1In0.SiLNe80_MERRYrC_Zjz3Pv8nzeK2qYS6o1gNdYoWd3pIaY7JsMo-sZKCyseZae3jXqMwhpSfyAvCM85UkzgsywPFx4oJK_KGt-eqzZbSEiDLXQb-3qsjNCDCqUYxRGfkGesodWvoh22oot196kSlnyLheH6k_TYgV8Ud84lwnAuwz8ydidaubue42vFMeYEfrIy99E2rhP2KJjcg0XGVzUh4RvDu1IR9BSSFN8zWjVcX5EOHEUzxt9CsioojiXKfdQ6YGGh7iLjb1qT8VWzu_CmewhuIFPHdDoTS0mJfulbykSWN3_5m_TFZpR7G3Ybwd5DVTfX9xLbwNJm_C3TL-Q");
request.AddJsonBody("{\"merchantCode\":\"MX6072\",\"payableCode\":\"9405967\",\"amount\":\"5000\",\"redirectUrl\":\"https://webpay-ui.k8.isw.la/demo-response\",\"customerId\":\"johndoe@gmail.com\",\"currencyCode\":\"566\",\"customerEmail\":\"johndoe@gmail.com\"}", false);
var response = await client.PostAsync(request);
*/