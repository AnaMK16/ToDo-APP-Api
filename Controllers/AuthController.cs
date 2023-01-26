using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Auth;
using TodoApp.Api.DB;
using TodoApp.Api.Models.Entities;
using TodoApp.Api.Models.Requests;
using TodoApp.Api.Repositories;

namespace TodoApp.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenGenerator _tokenGenerator;
    private UserManager<UserEntity> _userManager;
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly ISendEmailRequestRepository _sendEmailRequestRepository;
    public AuthController(TokenGenerator tokenGenerator, UserManager<UserEntity> userManager, AppDbContext db, IConfiguration configuration, ISendEmailRequestRepository sendEmailRequestRepository)
    {
        _tokenGenerator = tokenGenerator;
        _userManager = userManager;
        _db = db;
        _sendEmailRequestRepository = sendEmailRequestRepository;
        _configuration = configuration;
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
        entity.UserName = request.Email;
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
        
        return Ok(_tokenGenerator.Generate(user.Id.ToString()));
    }
/*
    [HttpPost("generate-password-reset-token")]
    public async Task<string> GeneratePasswordResetToken([FromBody]GeneratePasswordResetRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        // TODO: Send token to request.Email address
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }
    
    [HttpPost("reset-password")]
    public async Task<bool> ResetPassword([FromBody]ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        var result = await _userManager.ResetPasswordAsync(user, request.PasswordResetToken, request.NewPassword);
        return result.Succeeded;
    }*/
    [HttpPost("request-password-request")]
    public async Task<IActionResult> RequestPasswordReset([FromBody]GeneratePasswordResetRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // User not found
            return StatusCode(404, "User not found");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var sendEmailRequestEntity = new SendEmailRequestEntity();
        sendEmailRequestEntity.ToAddress = request.Email;
        sendEmailRequestEntity.Status = SendEmailRequestStatus.New;
        sendEmailRequestEntity.CreatedAt = DateTime.Now;
        var url = _configuration["PasswordResetUrl"]!
            .Replace("{userId}", user.Id.ToString())
            .Replace("{token}", token);
        var resetUrl = $"<a href=\"{url}\">Reset password</a>";
        sendEmailRequestEntity.Body = $"Hello, your password reset link is: {resetUrl}";
        
        _sendEmailRequestRepository.Insert(sendEmailRequestEntity);
        await _sendEmailRequestRepository.SaveChangesAsync();
        
        return Ok();

    } 
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserID.ToString());
        if (user == null)
        {
            return NotFound("User not found");
        }
        var resetResult = await _userManager.ResetPasswordAsync(user, request.PasswordResetToken, request.Password);
        
        if (!resetResult.Succeeded)
        {
            var firstError = resetResult.Errors.First();
            return StatusCode(500, firstError.Description);
        }
        
        return Ok();
    }
    
}