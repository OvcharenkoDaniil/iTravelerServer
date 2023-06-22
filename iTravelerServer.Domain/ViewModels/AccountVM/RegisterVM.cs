using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.ViewModels.AccountVM
{
    public class RegisterVM
    {
        
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string PhoneNumber { get; set; }
        
        public string Password { get; set; }
        
        public string Email { get; set; }
        
    }
}