using FarmLink.IndentityService.Models.RequestModels;
using FarmLink.Shared.Enumarations;
using FarmLink.Shared.Services;
using IdentityService.Extensions;
using IdentityService.Repositories.Interfaces;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        }

        [HttpPost("AddUser")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserResponseModel>> CreateUserAsync(UserRequestModel user)
        {
            var result = await ExecuteWithLogging(async () =>
            {
                var hash = _hashingService.GenerateSaltAndHash(user.Password);
                string otp = "".GenerateOtp();
                user.Salt = hash.Salt;
                user.Password = hash.Hash;
                user.UserState = State.Inactive;
                user.IsPhoneVerified = false;
                user.IsEmailVerified = false;
                user.Otp = otp;
                var newUser = await _repository.AddUserAsync(user);
                string subject = $"Your Email Verification Code {otp}";
                string body = $"Hello {user.FirstName},\n\n your verification code is: {otp}"; ;
                await _emailService.SendEmail(user.Email, subject, body);
                return newUser;
            });
            if (result.Data == null && !result.Error)
                return Unauthorized(result);
            if (result.Error)
                return StatusCode(500, result);
            return Ok(result);
        }

        [HttpPost("Login")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserResponseModel>> Login(LoginRequestModel model)
        {
            var user = await _repository.GetUserByEmailAsync(model.Email);
            if (user.Data == null&&!user.Error)
            {
                return NotFound(user);
            }
            if(user.Error)
            {
                return StatusCode(500, user); 
            }
            else
            {
                if (user.Data.UserState == State.Inactive)
                {
                    return Unauthorized(user);
                }
                else
                {
                    model.Password = _hashingService.HashPassword(model.Password, user.Data.Salt);
                    var result =await ExecuteWithLogging(()=> _repository.LoginAsync(model));
                    if(result==null)
                        return NotFound(user);
                  return Ok(result);
                }
            }
        }
        [HttpPost("SendOtpToVerifyUserEmail")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserResponseModel>> SendOtpToVerifyUserEmail(string Email)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var user = await _repository.GetUserByEmailAsync(Email);
            if (user == null)
            {
                return NotFound("User does not exist");
            }
            else
            {
                if (user.Data.UserState == State.Inactive)
                {
                    return Unauthorized("Your account is inactive. Call support to activate your account");
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
                    var result = await _repository.UpdateUserOtpAsync(user.Data);
                    if (result == 1)
                    {
                        return Ok(user);
                    }
                    return StatusCode(500, "Something went wrong");
                }
            }
        }
        [HttpPost("VerifyOtpAndActivateUser")]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<UserResponseModel>> VerifyOtpAndActivateUser(VerifyOtpRequestModel request)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);

            var user = await _repository.GetUserByIdAsync(request.UserId);
            if (user.Data != null)
            {
                if (user.Data.Otp == request.Otp)
                {
                    user.Data.UserState = State.Active;
                    user.Data.IsEmailVerified = true;
                    var result = await ExecuteWithLogging(async () => await _repository.UpdateUserAsync(user.Data));

                    return Ok(result);
                }
                else
                {
                    return StatusCode(422, "The OTPs do not match");
                }
            }
            else
            {
                return NotFound("The user is not found");
            }
        }

        [HttpPost("UpdatePassword")]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<UserResponseModel>> UpdatePassword(UpdatePasswordRequest Request)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);

            var result = await _repository.GetUserByEmailAsync(Request.Email);
            var user = result.Data;
            if (user != null)
            {
                if (user.Otp == Request.Otp)
                {
                    var hash = _hashingService.GenerateSaltAndHash(Request.Password);
                    user.Password = hash.Hash;
                    user.Salt = hash.Salt;
                    var update = await ExecuteWithLogging(async () => await _repository.UpdateUserPasswordAsync(user));

                    return Ok(update);
                }
                else
                {
                    return StatusCode(422, "The OTP does not match");
                }
            }
            else
            {
                return NotFound("The user does not exist");
            }
        }

        [HttpGet("GetAllUsers")]
        [ProducesResponseType(typeof(IEnumerable<UserResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> GetAllUsers()
        {
            var result = await ExecuteWithLogging(() => _repository.GetUsersAsync());
            var first = result.FirstOrDefault();
            if (first.Data == null && !first.Error)
                return NotFound(result);
            if (first.Error)
                return StatusCode(500, result);
           return Ok(result);
        }

        [HttpGet("GetUserById")]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserResponseModel>> GetUserById(string Id)
        {
            var result = await ExecuteWithLogging(async () =>
            {
                var user = await _repository.GetUserByIdAsync(Id);
                if (user == null)
                {
                    return null;
                }
                return user;
            });

            if (result == null)
            {
                return NotFound("User not found");
            }

            return Ok(result);
        }

        [HttpGet("GetUserByEmail")]
        [ProducesResponseType(typeof(UserResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserResponseModel>> GetUserByEmail(string Email)
        {
            var result = await ExecuteWithLogging(async () =>
            {
                var user = await _repository.GetUserByEmailAsync(Email);
                if (user == null)
                {
                    return null;
                }
                return user;
            });

            if (result == null)
            {
                return NotFound("User not found");
            }

            return Ok(result);
        }


        #region Helpers
        private async Task<UserResponseModel> ExecuteWithLogging(Func<Task<UserResponseModel>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;
        }

        private async Task<IEnumerable<UserResponseModel>> ExecuteWithLogging(Func<Task<IEnumerable<UserResponseModel>>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);
            var result = await action.Invoke();
            _logger.LogInformation("[END] " + logName);
            return result;
        }

        private async Task<ActionResult<UserResponseModel>> ExecuteActionAsync(Func<Task<long>> action)
        {
            var logName = MethodBase.GetCurrentMethod()?.Name;
            _logger.LogInformation("[BEGIN] " + logName);

            var result = await action();

            _logger.LogInformation("[END] " + logName);

            if (result == 1)
                return Ok("Successfully completed the action");
            if (result == 0)
                return NotFound("Action was not completed");
            else
                return StatusCode(500, $"Multiple actions were completed: {logName}, number of actions completed: {result}");
        }
        #endregion
    }
}

