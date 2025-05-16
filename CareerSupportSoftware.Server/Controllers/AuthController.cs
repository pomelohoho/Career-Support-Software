// Controllers/AuthController.cs
using CareerSupportSoftware.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager; 
    private readonly SignInManager<ApiUser> _signInManager;
    private readonly AuthService _authService;

    public AuthController(
        UserManager<ApiUser> userManager,
        SignInManager<ApiUser> signInManager,
        RoleManager<IdentityRole> roleManager, 
        AuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new ApiUser { Email = request.Email, UserName = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (!await _roleManager.RoleExistsAsync("Student"))
        {
            return StatusCode(500, "Role 'Student' not configured");
        }

        await _userManager.AddToRoleAsync(user, "Student");
        return Ok(new { Message = "User created successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return Unauthorized("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded) return Unauthorized("Invalid credentials");

        var roles = await _userManager.GetRolesAsync(user);

        user.RefreshToken = _authService.GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(
            _authService.GetRefreshTokenExpiryDays());

        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            Token = _authService.GenerateJwtToken(user, roles.ToList()),
            RefreshToken = user.RefreshToken,
            ExpiresIn = TimeSpan.FromMinutes(
                _authService.GetJwtExpirationMinutes()).TotalSeconds
        });
    }
}

public record RegisterRequest(string Email, string Password);
public record LoginRequest(string Email, string Password);