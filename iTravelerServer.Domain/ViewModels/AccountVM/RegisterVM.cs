using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.ViewModels.AccountVM
{
    public class RegisterVM
    {
        
        public string userName { get; set; }
        
        public string Password { get; set; }
        
        public string Email { get; set; }
        
    }
}