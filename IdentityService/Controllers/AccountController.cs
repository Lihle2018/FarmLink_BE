using FarmLink.Shared.Entiities;
using FarmLink.Shared.RequestModels;
using FarmLink.Shared.ResponseModel;
using FarmLink.Shared.Services;
using IdentityService.Extensions;
using IdentityService.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailService _emailService;
        private readonly IHashingService _hashingService;
        public AccountController(IUserRepository repository, ILogger<AccountController> logger, IEmailService emailService, IHashingService hashingService)
        {
            _repository = repository;
            _logger = logger;
            _emailService = emailService;
            _hashingService = hashingService;
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> CreateUserAsync(UserRequestModel user)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var hash = _hashingService.GenerateSaltAndHash(user.Password);
            string otp = "".GenerateOtp();
            user.Salt = hash.Salt;
            user.Password = hash.Hash;
            user.UserState = State.Inactive;
            user.IsPhoneVerified = false;
            user.IsEmailVerified = false;
            user.Otp = otp;
            var result = new UserResponseModel(await _repository.AddUserAsync(user));

            string subject = $"Your Email Verification Code {otp}";
            string body = $"Hello {user.FirstName},\n\n your verification code is: {otp}"; ;
            await _emailService.SendEmail(user.Email, subject, body);
            _logger.LogInformation("[END] " + logName);
            return new JsonResult(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var user = await _repository.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                return new JsonResult("User does not exist")
                {
                    StatusCode = 401
                };

            }
            else
            {
                if (user.UserState == State.Inactive)
                {
                    return new JsonResult("The user account is inactive. Please verify your email or call support")
                    {
                        StatusCode = 401
                    };
                }
                else
                {
                    model.Password = _hashingService.HashPassword(model.Password, user.Salt);
                    var result = new UserResponseModel(await _repository.LoginAsync(model));
                    _logger.LogInformation("[END] " + logName);
                    return new JsonResult(result);
                }
            }
        }
        [HttpPost("SendOtpToVerifyUserEmail")]
        public async Task<IActionResult> SendOtpToVerifyUserEmail(string Email)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);

            var user = await _repository.GetUserByEmailAsync(Email);
            if (user == null)
            {
                return new JsonResult("User does not exist")
                {
                    StatusCode = 401
                };
            }
            else
            {
                if (user.UserState == State.Inactive)
                {
                    return new JsonResult("Your account is inactive. Call support to activate your account")
                    {
                        StatusCode = 401
                    };
                }
                else
                {
                    string otp = "".GenerateOtp();
                    string subject = $"Your OTP: {otp}";
                    string body = $"Dear recipient," +
                        $"\n\nPlease find below the details of your important notification.\n\nOTP: {otp}\n\n" +
                        $"Best regards," +
                        $"\nX-Labs-24 solutions";
                    await _emailService.SendEmail(Email, subject, body);
                    var result = await _repository.UpdateUserOtpAsync(user);
                    _logger.LogInformation("[END] " + logName);
                    if (result == 1)
                    {
                        return new JsonResult(new UserResponseModel(user));
                    }
                    return new JsonResult("Something went wrong")
                    {
                        StatusCode = 404
                    };
                }
            }
        }
        [HttpPost("VerifyOtpAndActivateUser")]
        public async Task<IActionResult> VerifyOtpAndActivateUser(VerifyOtpRequestModel request)
        {

            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            User user;
            user = await _repository.GetUserByIdAsync(request.UserId);
            if (user != null)
            {
                if (user.Otp == request.Otp)
                {
                    user.UserState = State.Active;
                    user.IsEmailVerified = true;
                    var result = new UserResponseModel(await _repository.UpdateUserAsync(user));

                    return new JsonResult(result);
                }
                else
                {
                    return new JsonResult("The otps do not match")
                    {
                        StatusCode = 422
                    };
                }
            }
            else
            {
                return new JsonResult("The user is not found")
                {
                    StatusCode = 401
                };
            }
        }
        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest Request)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var user = await _repository.GetUserByEmailAsync(Request.Email);
            if (user != null)
            {
                if (user.Otp == Request.Otp)
                {
                    var hash = _hashingService.GenerateSaltAndHash(Request.Password);
                    user.Password = hash.Hash;
                    user.Salt = hash.Salt;
                    var result = new UserResponseModel(await _repository.UpdateUserPasswordAsync(user));
                    return new JsonResult(result);
                }
                else
                {
                    return new JsonResult("The otp does no match")
                    {
                        StatusCode = 422
                    };
                }
            }
            else
            {
                return new JsonResult("The user does not exist")
                {
                    StatusCode = 401
                };
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var users = await _repository.GetAllUsersAsync();
            var result = users.Select(x => new UserResponseModel(x)).ToList();
            _logger.LogInformation("[END] " + logName);
            return new JsonResult(result);
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = new UserResponseModel(await _repository.GetUserByIdAsync(Id));
            _logger.LogInformation("[END] " + logName);
            return new JsonResult(result);
        }
        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string Email)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = new UserResponseModel(await _repository.GetUserByEmailAsync(Email));
            _logger.LogInformation("[END] " + logName);
            return new JsonResult(result);
        }
    }
}

