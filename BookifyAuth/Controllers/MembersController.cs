using System.Reflection;
using BookifyAuth.DTO;
using BookifyAuth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookifyAuth.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly IUserService _userService;

        public MembersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result =  _userService.RegisterUser(request);
                    if (!result.Success)
                        return BadRequest(new
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Registration Faild!",
                            Errors = result.Errors
                        });
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "User registered successfully",
                        Data = result.Data
                    });
                }
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "ModelStateError",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _userService.Login(request);
                    if (!result.Success)
                        return BadRequest(new
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Login Faild!",
                            Errors = result.Errors
                        });
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "User logged in successfully",
                        Data = result.Data
                    });
                }
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "ModelStateError",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser([FromRoute] int userId)
        {
            try
            {
                var result = _userService.GetUserById(userId);
                if (result == null)
                    return NotFound();
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User registered successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet("allUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "All users retrieved successfully",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }


        [HttpPost("upgradeMembership")]
        public IActionResult UpgradeMembership([FromBody] MembershipUpgradeRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _userService.UpgradeMembershipTier(request);
                    if (!result.Success)
                        return BadRequest(new
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Upgrade Faild!",
                            Errors = result.Errors
                        });
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Membership Upgrade successfully",
                        Data = result.Data
                    });
                }
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "ModelStateError",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                });
            }
        }

    }
}
