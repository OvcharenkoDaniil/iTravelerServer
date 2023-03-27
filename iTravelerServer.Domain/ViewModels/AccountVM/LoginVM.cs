using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.ViewModels.AccountVM
{
    public class LoginRequestVM
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
    
    
}   