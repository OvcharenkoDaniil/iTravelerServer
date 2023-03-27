using System.Collections;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.AccountVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iTravelerServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;
    private readonly IFlightService _flightService;


    public AccountController(IAccountService accountService, IConfiguration configuration, IFlightService flightService)
    {
        //_authOptions = authOptions;
        _accountService = accountService;
        _configuration = configuration;
        _flightService = flightService;
    }


    // [Authorize]
    // [HttpPut("UpdateAccount")]
    // public async Task<bool> UpdateAccount(Account account)
    // {
    //     var response = await _accountService.UpdateAccount(account);
    //
    //     if (response.StatusCode == Domain.Enum.StatusCode.OK)
    //     {
    //         return true;
    //     }
    //
    //     return false;
    // }
    
    [Authorize]
    [HttpPut("ChangePassword")]
    public async Task<bool> ChangePassword(ChangePasswordVM userData)
    {
        var response = await _accountService.ChangePassword(userData);

        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return true;
        }

        return false;
    }
    
    //[Authorize(Roles = "Admin")]
    [HttpPost("DeleteAccount")]
    public async Task<Boolean> DeleteAccount(AccountDeleteDataVM userData)
    {
        var response = await _accountService.DeleteAccount(userData);

        if (response.StatusCode == Domain.Enum.StatusCode.OK)
        {
            return true;
        }
        return false;
    }


    [Authorize(Roles = "Admin")]
    [Route("GetAllAccounts")]
    [HttpGet]
    public async Task<List<UserDataVM>> GetAllAccounts()
    {
        var accountResponse = await _accountService.GetAccounts();
        var accountList = new List<UserDataVM>();

        if (accountResponse.StatusCode == Domain.Enum.StatusCode.OK)
        {
            foreach (var account in accountResponse.Data)
            {
                accountList.Add(
                    new UserDataVM()
                    {
                        email = account.Email,
                        name = account.Username,
                        role = account.Role.ToString(),
                        account_id = account.Account_id
                    }
                );
            }
            return accountList;
        }
        return null;
    }
    
    
    
}