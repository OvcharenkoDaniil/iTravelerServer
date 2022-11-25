using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using iTraveler.Auth;
using iTravelerServer.Domain;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
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
    //private readonly IOptions<AuthOptions> _authOptions;
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;
    public AuthController(IOptions<AuthOptions> authOptions, IAccountService accountService, IConfiguration configuration)
    {
        //_authOptions = authOptions;
        _accountService = accountService;
        _configuration = configuration;
    }

    //[HttpPost,Route("login")]
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestVM user)
    {
        var response = await _accountService.Login(user);
        
        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            
            //loginData.Email = response.Data.Email;

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
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordVM userData)
    {
        var response = await _accountService.ChangePassword(userData);

        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return Ok(response.Description);
        }
        return BadRequest(response.Description);
    }


    // if (user == null)
    // {
    //     return BadRequest("Invalid client request");
    // }
    //
    // if (user.UserName == "user1" && user.Password == "123" )
    // {
    //     var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("iTravelerSecretKey"));
    //     var signingCredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
    //     
    //     var tokenOptions = new JwtSecurityToken(
    //         issuer: "https://localhost:7138",
    //         audience:"https://localhost:7138",
    //         claims: new List<Claim>(),
    //         expires: DateTime.Now.AddMinutes(5),
    //         signingCredentials: signingCredentials
    //     );
    //
    //     var tokenaString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    //     return Ok(new { Token = tokenaString });
    // }
    // return Unauthorized();


    //var user = AuthenticateUser(reqest.Email, reqest.Password);
    // if (user != null)
    // {
    //     var token = GenerateJWT(user);
    //
    //     return Ok(new
    //     {
    //         acess_token = token
    //     });
    // }
    // GET
    // private List<User> Accounts => new List<User>
    // {
    //     new User()
    //     {
    //         Id = Guid.Parse("b274ca9f-8217-48cd-b04c-8b42a0641910"),
    //         Email = "f",
    //         Password = "f",
    //         Roles = new Role[] { Role.User }
    //     },
    //     new User()
    //     {
    //         Id = Guid.Parse("3b878273-f795-4d22-801f-f78b963d5523"),
    //         Email = "s@gmail.com",
    //         Password = "s",
    //         Roles = new Role[] { Role.User }
    //         
    //     },
    //     new User()
    //     {
    //         Id = Guid.Parse("71fc9fc8-db21-4e33-aa55-2828a2fc9de6"),
    //         Email = "d",
    //         Password = "d",
    //         Roles = new Role[] { Role.Admin }
    //     }
    // };


    // private User AuthenticateUser(string reqestEmail, string reqestPassword)
    // {
    //     return Accounts.SingleOrDefault(u => u.Email == reqestEmail && u.Password == reqestPassword);
    // }
    //
    // private string GenerateJWT(User user)
    // {
    //     var authParams = _authOptions.Value;
    //     var securityKey = authParams.GetSymmetricSecurityKey();
    //     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    //
    //     var claims = new List<Claim>()
    //     {
    //         new Claim(JwtRegisteredClaimNames.Email, user.Email),
    //         new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
    //     };
    //
    //     foreach (var role in user.Roles)
    //     {
    //         claims.Add(new Claim("role", role.ToString()));
    //     }
    //
    //     var token = new JwtSecurityToken(
    //         authParams.Issuer,
    //         authParams.Audience,
    //         claims,
    //         expires:DateTime.Now.AddSeconds(authParams.TokenLifetime),
    //         signingCredentials:credentials);
    //     
    //     return new JwtSecurityTokenHandler().WriteToken(token);
    //
    // }
}