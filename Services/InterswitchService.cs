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



    public class InterswitchAuthService()
    {
        public async Task<string> GetAccessTokensync()
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{"IKIA920656DA4CDDF5FBEA6E470F503ACBE3326F89EA"}:{"uJ36GtDU5uQT3hy"}"));
            var options = new RestClientOptions("https://apps.qa.interswitchng.com/passport/oauth/token?grant_type=client_credentials");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Basic {credentials}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            var response = await client.PostAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Could not retrieve access code! - {response.ErrorMessage}");
            }

            var _response = JsonSerializer.Deserialize<Dictionary<string, object>>(response.Content);
            if (_response != null && _response.TryGetValue("access_token", out object value))
            {
                return value.ToString();
            }

            // if (_response != null && _response.ContainsKey("access_token"))
            // {
            //     return _response["access_token"].ToString();
            // }

            throw new Exception("Access token not found in the response!");




        }


    }
    public class InterswitchService(IConfiguration configuration, InterswitchAuthService interswitchAuthService)
    {
        private readonly IConfiguration configuration = configuration;
        // private readonly InterswitchAuthService interswitchAuthService = interswitchAuthService;

        public async Task<PaymentInitializationResponseDto> InitializePaymentAsync(string amount, string customerId, string customerEmail)
        {
            try
            {
                var accessToken = await interswitchAuthService.GetAccessTokensync();
                Console.WriteLine(accessToken);

                if (string.IsNullOrEmpty(accessToken))
                    throw new Exception("Could not retrieve access token");


                var options = new RestClientOptions("https://qa.interswitchng.com/paymentgateway/api/v1/paybill");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                var PaybillRequestDto = new PaybillRequestDto
                {
                    MerchantCode = "MX6072",
                    PayableCode = "9405967",
                    Amount = amount,
                    RedirectUrl = "https://expert.joinebo.app",
                    CustomerId = customerId,
                    CurrencyCode = "566",
                    CustomerEmail = customerEmail
                };
                request.AddJsonBody(PaybillRequestDto);
                var response = await client.PostAsync(request);
                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"Payment could not be initialized: ");
                }
                var _json = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var paymentResponse = new PaymentInitializationResponseDto
                {
                    PayableCode = _json.GetProperty("payableCode").GetString(),
                    Amount = _json.GetProperty("amount").GetInt32().ToString(),
                    TransactionReference = _json.GetProperty("reference").GetString(),
                    RedirectUrl = _json.GetProperty("redirectUrl").GetString(),
                    PaymentUrl = _json.GetProperty("paymentUrl").GetString(),
                    CustomerId = _json.GetProperty("customerId").GetString()

                };
                return paymentResponse;

            }
            catch (Exception Err)
            {

                throw new Exception($"An error occurred while initializing the payment: {Err.Message}");
            }


        }

        //verify that payment was successful
        public async Task<PaymentVerificationResponseDto> VerifyPaymentAsync(string transactionReference, decimal amount)
        {
            try

            {
                // var accessToken = await interswitchAuthService.GetAccessTokensync();
                // Console.WriteLine("start 1111");

                // if (string.IsNullOrEmpty(accessToken))
                // {
                //     throw new Exception("Couldn't get access token");
                // }

                // // Base URL for the API
                // var options = new RestClientOptions("https://qa.interswitchng.com/collections/api/v1/gettransaction.json");
                // var client = new RestClient(options);

                // // Construct the verification URL with query parameters
                // var verificationUrl = $"?merchantcode=MX200816&transactionreference={transactionReference}&amount={amount}";

                // var request = new RestRequest(verificationUrl, Method.Get);

                // // Add necessary headers
                // request.AddHeader("Content-Type", "application/json");
                // request.AddHeader("Authorization", $"Bearer {accessToken}");

                // Console.WriteLine($"request====>{request}");

                // // Make the GET request
                // var response = await client.ExecuteAsync(request);
                // Console.WriteLine($"response====>{response.Content}");

                // // Check if the response has content
                // if (string.IsNullOrEmpty(response.Content))
                // {
                //     throw new Exception($"No content was returned from the server: {response.Content}");
                // }

                // // Deserialize the response content to your DTO
                // var verificationResponse = JsonSerializer.Deserialize<PaymentVerificationResponseDto>(response.Content);

                // if (verificationResponse.Amount == 0 || verificationResponse.ResponseCode  == "Z25"){
                //     throw new Exception($"Payment verification failed: {verificationResponse.ResponseDescription}, Response Code: {verificationResponse.ResponseCode}");
      
                // }

                // return verificationResponse;

                var accessToken = await interswitchAuthService.GetAccessTokensync();
                Console.WriteLine("start 1111");

                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new Exception("Couldn't get acces token");
                }
                // LIVE BASE URL: https://webpay.interswitchng.com
                var options = new RestClientOptions($"https://qa.interswitchng.com/collections/api/v1/gettransaction.json?merchantcode=MX200816&transactionreference={transactionReference}&amount={amount}");
                var client = new RestClient(options);

                var request = new RestRequest("");

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                Console.WriteLine($"request====>{request}");
                var response = await client.GetAsync(request);
                Console.WriteLine($"response====>{response.Content}");

                if (string.IsNullOrEmpty(response.Content))
                {
                    throw new Exception($"No content was returned from the server: {response.Content}");
                }

                var verificationResponse = JsonSerializer.Deserialize<PaymentVerificationResponseDto>(response.Content);

                if (verificationResponse.Amount == 0 || verificationResponse.ResponseCode  == "Z25"){
                    throw new Exception($"Payment verification failed: {verificationResponse.ResponseDescription}, Response Code: {verificationResponse.ResponseCode}");
      
                }
                return verificationResponse;


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