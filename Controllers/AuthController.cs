using Microsoft.AspNetCore.Mvc;
using PortfolioAdmin.Api.DTOs;
using PortfolioAdmin.Api.Services;

namespace PortfolioAdmin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IJwtService _jwtService;

    public AuthController(IAuthService authService, IJwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var user = await _authService.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = _jwtService.GenerateToken(user);

            var response = new LoginResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    LastLoginAt = user.LastLoginAt
                }
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto request)
    {
        try
        {
            var user = await _authService.CreateUserAsync(request.Email, request.Password, request.Role);

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                LastLoginAt = user.LastLoginAt
            };

            return CreatedAtAction(nameof(Register), new { id = user.Id }, userDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message });
        }
    }

    [HttpGet("me")]
    public ActionResult<object> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            userId = userIdClaim,
            email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
            role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }
}