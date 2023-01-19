using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Auth;
using TodoApp.Api.Models.Entities;
using TodoApp.Api.Models.Requests;

namespace TodoApp.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenGenerator _tokenGenerator;
    private UserManager<UserEntity> _userManager;
    public AuthController(TokenGenerator tokenGenerator, UserManager<UserEntity> userManager)
    {
        _tokenGenerator = tokenGenerator;
        _userManager = userManager;
    }
    
    //Register
    //RequestPasswordReset
    //ResetPassword
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterUserRequest request)
    {
        // Create
        var entity = new UserEntity();
        entity.Email = request.Email;
        var result = await _userManager.CreateAsync(entity, request.Password);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.First();
            return BadRequest(firstError.Description);
        }

        // var token = await _userManager.GeneratePasswordResetTokenAsync(entity);
        
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginRequest request)
    {
        // TODO:Check user credentials...
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // User not found
            return NotFound("User not found");
        }
        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isCorrectPassword)
        {
            return BadRequest("Invalid email or password");
        }
        
        return Ok(_tokenGenerator.Generate(request.Email));
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        UserEntity user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // User not found
            return NotFound("User not found");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

        return Ok();


    }
    
}