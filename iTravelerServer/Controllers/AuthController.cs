
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.ViewModels.AccountVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace iTravelerServer.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;
    public AuthController( IAccountService accountService, IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
    }


    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestVM user)
    {
        var response = await _accountService.Login(user);
        
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            var token = _accountService.CreateJWT(response.Data);
            
            return Ok(new{
                access_token = token
            });
        }
        return Unauthorized(response);
    }
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterVM registerData)
    {
        var response = await _accountService.Register(registerData);

        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
   
    
}