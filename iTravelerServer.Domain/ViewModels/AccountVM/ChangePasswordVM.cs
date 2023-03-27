using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.ViewModels.AccountVM
{
    public class ChangePasswordVM
    {
        public string password { get; set; }
        public string newPassword { get; set; }
        public string email { get; set; }
    }
}