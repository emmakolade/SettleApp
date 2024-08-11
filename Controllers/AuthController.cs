using Azure;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Settle_App.Helpers;
using Settle_App.Models.Domain;
using Settle_App.Models.DTO;
using Settle_App.Repositories;
using Settle_App.Roles;

namespace Settle_App.Controllers
{
    [Route("auth/")]
    [ApiController]
    public class AuthController(UserManager<SettleAppUser> userManager, IJWTRepository jWTRepository, IWalletRepository walletRepository, StringHelper stringHelper) : ControllerBase
    {
        private readonly UserManager<SettleAppUser> userManager = userManager;
        private readonly IJWTRepository jWTRepository = jWTRepository;
        private readonly IWalletRepository walletRepository = walletRepository;
        private readonly StringHelper stringHelper = stringHelper;

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto RegisterDto)
        {
            //check if user exists 
            var userExists = await userManager.FindByEmailAsync(RegisterDto.Email);

            //TODO: implement repository for retrieving user by username
            if (userExists != null)
            {
                if (userExists?.SettleAppUserName == RegisterDto.SettleAppUserName)
                    return StatusCode(StatusCodes.Status400BadRequest, new EndpointResponse { Status = "Error", Message = "username already exists!" });

            }

            if (userExists?.Email == RegisterDto.Email)
                return StatusCode(StatusCodes.Status400BadRequest, new EndpointResponse { Status = "Error", Message = "email already exists!" });



            if (RegisterDto.Password != RegisterDto.ConfirmPassword)
                return StatusCode(StatusCodes.Status400BadRequest, new EndpointResponse { Status = "Error", Message = "your password doesnt match" });
            Console.WriteLine("passed 3");

            var identityUser = new SettleAppUser
            {
                SettleAppFullName = RegisterDto.SettleAppFullName.ToLower(),
                SettleAppUserName = RegisterDto.SettleAppUserName,
                PhoneNumber = RegisterDto.PhoneNumber,
                Email = RegisterDto.Email,
                UserName = RegisterDto.Email,
                NormalizedEmail = RegisterDto.Email
            };
            Console.WriteLine("passed 4");

            Console.WriteLine(identityUser);

            var identityResult = await userManager.CreateAsync(identityUser, RegisterDto.Password);
            Console.WriteLine("passed 5");

            if (identityResult.Succeeded)

            {
                Console.WriteLine("passed 6");

                await userManager.AddToRoleAsync(identityUser, SettleAppUserRoles.SettleAppUser);

                await walletRepository.CreateWalletAsync(identityUser);
                return Ok(new EndpointResponse { Status = "Success", Message = "Account Created Successfully!!" });

            }
            return StatusCode(StatusCodes.Status400BadRequest, new EndpointResponse { Status = "Error", Message = "Account creation failed! Please check your details and try again." });

        }



        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] LoginDto LoginDto)
        {
            var user = await userManager.FindByEmailAsync(LoginDto.Email);
            if (user != null)
            {
                var checkPassword = await userManager.CheckPasswordAsync(user, LoginDto.Password);
                if (checkPassword)
                {
                    var getUserRole = await userManager.GetRolesAsync(user);

                    if (getUserRole != null)
                    {
                        var accessToken = jWTRepository.GenerateAccessToken(user, getUserRole?.ToString());
                        var refreshToken = jWTRepository.GenerateRefreshToken();
                        user.RefreshToken = refreshToken;
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                        await userManager.UpdateAsync(user);

                        var res = new AccessTokenResponse
                        {
                            AccessToken = accessToken.Token,
                            RefreshToken = refreshToken,
                            ExpiresIn = accessToken.Expiry,
                        };
                        return Ok(res);
                    }
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new EndpointResponse { Status = "Error", Message = "email or password is incorrect, please check again" });

        }
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto RefreshTokenDto)
        {
            // Find the user associated with the refresh token
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == RefreshTokenDto.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new EndpointResponse { Status = "Error", Message = "Invalid client request" });
            }

            // Generate new tokens
            var newAccessToken = jWTRepository.GenerateAccessToken(user, (await userManager.GetRolesAsync(user)).FirstOrDefault());
            var newRefreshToken = jWTRepository.GenerateRefreshToken();

            // Update the user with the new refresh token and its expiry time
            user.RefreshToken = newRefreshToken;
            // Ensure RefreshTokenExpiryTime is not null before performing operations
            if (user.RefreshTokenExpiryTime.HasValue)
            {
                var refreshTokenExpiryTime = user.RefreshTokenExpiryTime.Value; // Get the non-nullable DateTime

                // Update the expiry time
                refreshTokenExpiryTime = DateTime.Now.AddDays(7); // Refresh token expiry time

                // Update the user
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime; // Assuming RefreshTokenExpiryTime is a non-nullable DateTime
            }
            else
            {
                // Handle the case where RefreshTokenExpiryTime is null, if necessary
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Set to a new value if it was null
            }

            // Save changes
            await userManager.UpdateAsync(user);

            // Convert DateTime to Unix Timestamp for expiry
            var expiryTimestamp = new DateTimeOffset(DateTime.Now.AddDays(7)).ToUnixTimeSeconds(); // Using new expiry time

            // Return the new tokens
            return Ok(new AccessTokenResponse
            {
                AccessToken = newAccessToken.Token,
                RefreshToken = newRefreshToken,
                ExpiresIn = expiryTimestamp // Return the correct expiry timestamp
            });
        }



    }
}



//[HttpPost]
//[Route("assign-role")]
//[Authorize(Roles = "SettleAppAdmin")]
//public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO assignRoleDTO)
//{
//    var user = await userManager.FindByEmailAsync(assignRoleDTO.Email);
//    if (user == null)
//        return StatusCode(StatusCodes.Status404NotFound, new EndpointResponse { Status = "Error", Message = "User not found!" });

//    var roleExists = await roleManager.RoleExistsAsync(assignRoleDTO.Role);
//    if (!roleExists)
//        return StatusCode(StatusCodes.Status400BadRequest, new EndpointResponse { Status = "Error", Message = "Role does not exist!" });

//    var result = await userManager.AddToRoleAsync(user, assignRoleDTO.Role);
//    if (result.Succeeded)
//        return Ok(new EndpointResponse { Status = "Success", Message = "Role assigned successfully!" });

//    return StatusCode(StatusCodes.Status400BadRequest, new EndpointResponse { Status = "Error", Message = "Role assignment failed!" });
//}
