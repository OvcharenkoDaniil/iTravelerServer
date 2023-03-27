using System.Collections.Generic;
using System.Threading.Tasks;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.AccountVM;


namespace iTravelerServer.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<IEnumerable<Account>>> GetAccounts();
        Task<BaseResponse<Account>> Login(LoginRequestVM loginReq);
        Task<BaseResponse<Account>> Register(RegisterVM registerViewModel);
        Task<BaseResponse<Account>> ChangePassword(ChangePasswordVM userData);
        Task<BaseResponse<Boolean>> DeleteAccount(AccountDeleteDataVM userData);

        string CreateJWT(Account account);
    }   
}