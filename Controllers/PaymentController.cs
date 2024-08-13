using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Settle_App.Helpers;
using Settle_App.Models.Domain;
using Settle_App.Models.DTO;
using Settle_App.Repositories;
using Settle_App.Services;

namespace Settle_App.Controllers
{
    [ApiController]
    [Route("payments/")]

    public class PaymentController(InterswitchService interswitchService, InterswitchAuthService interswitchAuthService, UserManager<SettleAppUser> userManager, IWalletRepository walletRepository, ITransactionRepository transactionRepository) : ControllerBase
    {
        private readonly InterswitchService interswitchService = interswitchService;
        private readonly InterswitchAuthService interswitchAuthService = interswitchAuthService;
        private readonly UserManager<SettleAppUser> userManager = userManager;
        private readonly IWalletRepository walletRepository = walletRepository;
        private readonly ITransactionRepository transactionRepository = transactionRepository;

        [HttpPost]
        [Route("initialize")]
        // [Authorize]
        public async Task<IActionResult> InitializePayment([FromBody] InterswitchPaymentInitializationRequestDto paymentInitializationRequest)
        {
            try
            {
                // var userId = User.Identity?.Name;
                // Console.WriteLine(userId);
                // var userId2 = User.FindFirstValue(ClaimTypes.NameIdentifier); // Safest way to get user ID
                // Console.WriteLine(userId2);
                // var userEmail = User.FindFirstValue(ClaimTypes.Email); // Get the user's email from claims
                var user = await userManager.FindByEmailAsync(paymentInitializationRequest.CustomerEmail);
                Console.WriteLine($"email=======>>>{user}");
                // if (user == null)
                // {
                //     return NotFound(new EndpointResponse { Status = "Error", Message = "User not found for this transaction" });
                // }
                var _response = await interswitchService.InitializePaymentAsync(
                    amount: paymentInitializationRequest.Amount,
                    customerId: user.Id,
                    customerEmail: user.Email
                );
                Console.WriteLine($"_response=======>>>{_response}");

                //update users transaction refrenece
                user.TransactionReference = _response.TransactionReference;
                var updateUser = await userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new EndpointResponse { Status = "Error", Message = "Could not Update Users Payment refernece" });
                }
                return Ok(_response);
            }
            catch (Exception err)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new EndpointResponse { Status = "Payment could not be initialized", Message = $"{err.Message}" });
            }

        }

        [HttpGet]
        [Route("verify/fund-wallet")]
        // [Authorize]
        public async Task<IActionResult> VerifyPayment([FromQuery] InterswitchPaymentVerificationRequestDto InterswitchPaymentVerificationRequestDto)
        {
            try
            {
                var user = await userManager.FindByEmailAsync("user3@example.com");
                // if (user == null || user.TransactionReference != InterswitchPaymentVerificationRequestDto.TransactionReference)
                // {
                //     return NotFound("User not found or transaction mismatch.");
                // }
                var userWallet = await walletRepository.GetWalletByIdAsync(user.Id);

                var verifyPayment = await interswitchService.VerifyPaymentAsync(
                    transactionReference: InterswitchPaymentVerificationRequestDto.TransactionReference,
                    amount: InterswitchPaymentVerificationRequestDto.Amount
                );

                if (verifyPayment != null)
                {
                    await walletRepository.UpdateWalletBalanceAsync(userWallet, InterswitchPaymentVerificationRequestDto.Amount);
                    await transactionRepository.CreateTransactionAsync(settleAppUser: user, amount: InterswitchPaymentVerificationRequestDto.Amount,
                                                                paymentGateway: InterswitchPaymentVerificationRequestDto.PaymentGateway, transactionStatus: TransactionStatus.Completed, transactionType: TransactionType.WalletFunding);

                    return Ok(new { message = $"Payment verified and wallet updated with {InterswitchPaymentVerificationRequestDto.Amount} successfully." });
                }
                return BadRequest(new { message = "Payment verification failed." });
            }
            catch (Exception err)
            {

                return StatusCode(StatusCodes.Status400BadRequest, new EndpointResponse { Status = "Payment could not be verified", Message = $"{err.Message}" });
            }

        }



    }
}



// public async Task<IActionResult> VerifyPayment([FromQuery] string transactionReference)
// {
//     var verificationResponse = await _interswitchService.VerifyPaymentAsync(transactionReference);

//     if (!verificationResponse.Status)
//     {
//         return BadRequest("Payment verification failed.");
//     }

//     var user = await _userManager.FindByIdAsync(User.Identity.Name);
//     if (user == null || user.PendingTransactionReference != transactionReference)
//     {
//         return NotFound("User not found or transaction mismatch.");
//     }

//     user.WalletBalance += verificationResponse.Amount / 100; // Convert from kobo to naira
//     user.PendingTransactionReference = null; // Clear the pending transaction reference
//     await _userManager.UpdateAsync(user);

//     return Ok("Wallet funded successfully.");