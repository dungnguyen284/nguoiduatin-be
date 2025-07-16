using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NDT.API.CustomedResponses;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.Services.Interfaces;
using NDT.BusinessModels.Entities;
using System.Text.Encodings.Web;

namespace NDT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJwtService jwtService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register(UserRequestDTO model)
        {
            try
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new ApiResponse<string>(400, "Registration failed", 
                        string.Join(", ", result.Errors.Select(e => e.Description))));
                }

                await _userManager.AddToRoleAsync(user, "Default");

                // Generate email confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Auth",
                    new { userId = user.Id, token = token }, Request.Scheme);

                var emailBody = $"Please confirm your email by clicking <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>here</a>.";
                await _emailService.SendEmailAsync(user.Email, "Confirm your email", emailBody);

                return Ok(new ApiResponse<string>(200, "Registration successful. Please check your email to confirm your account."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, ex.Message));
            }
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult<ApiResponse<string>>> ConfirmEmail(string userId, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                {
                    return BadRequest(new ApiResponse<string>(400, "Invalid email confirmation request"));
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(new ApiResponse<string>(400, "User not found"));
                }

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                {
                    return BadRequest(new ApiResponse<string>(400, "Email confirmation failed",
                        string.Join(", ", result.Errors.Select(e => e.Description))));
                }

                return Ok(new ApiResponse<string>(200, "Email confirmed successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(LoginRequestDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "Invalid email or password"));
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Unauthorized(new ApiResponse<string>(401, "Please confirm your email before logging in"));
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    return Unauthorized(new ApiResponse<string>(401, "Invalid email or password"));
                }

                var token = await _jwtService.GenerateToken(user);
                return Ok(new ApiResponse<string>(200, "Login successful", token));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, ex.Message));
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<string>>> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(new ApiResponse<string>(200, "Logout successful"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, ex.Message));
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<string>>> ForgotPassword(ForgotPasswordRequestDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Ok(new ApiResponse<string>(200, "If your email is registered, you will receive a password reset link"));
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "Auth",
                    new { email = model.Email, token = token }, Request.Scheme);

                var emailBody = $"Please reset your password by clicking <a href='{HtmlEncoder.Default.Encode(resetLink)}'>here</a>.";
                await _emailService.SendEmailAsync(user.Email, "Reset Password", emailBody);

                return Ok(new ApiResponse<string>(200, "If your email is registered, you will receive a password reset link"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, ex.Message));
            }
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponse<string>>> ChangePassword(ChangePasswordRequestDTO model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse<string>(401, "User not found"));
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(new ApiResponse<string>(400, "Password change failed",
                        string.Join(", ", result.Errors.Select(e => e.Description))));
                }

                return Ok(new ApiResponse<string>(200, "Password changed successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, ex.Message));
            }
        }
    }
} 