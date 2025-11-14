using System.IdentityModel.Tokens.Jwt;
using ApiWithRoles.DTOs;
using ApiWithRoles.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace ApiWithRoles.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class CreateUsersController : ControllerBase
{
    private readonly UserServices _userService;

    public CreateUsersController(UserServices userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register request)
    {
        var result = await _userService.RegisterUserAsync(request);
        if(result.Contains("Email already registered."))
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] Verify request)
    {
        var result = await _userService.VerifyUserAsync(request);
        if(result.Contains("already") || result.Contains("not") || result.Contains("expired") || 
            result.Contains("Invalid")) 
        {
            return BadRequest(result);
        }
    
        return Ok(result);
    }
    
    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword([FromBody] SetPassword request)
    {
        var result = await _userService.SetPasswordAsync(request);
        if(result.Contains("yet") || result.Contains("not")) 
        {
            return BadRequest(result);
        }
    
        return Ok(result);
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] Login request)
    {
        var result = _userService.LoginUser(request);
        if(result.Contains("yet") || result.Contains("not") || result.Contains("Invalid")) 
        {
            return BadRequest(result);
        }
        
        return Ok(new { Token = result });
    }
    
}