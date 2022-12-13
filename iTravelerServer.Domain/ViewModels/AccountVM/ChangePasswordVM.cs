using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.ViewModels.AccountVM
{
    public class ChangePasswordVM
    {
        //public string UserName { get; set; }
        
        // [Required(ErrorMessage = "Введите пароль")]
        // [DataType(DataType.Password)]
        // [Display(Name = "Пароль")]
        // [MinLength(5, ErrorMessage = "Пароль должен быть больше или равен 5 символов")]
        public string password { get; set; }
        public string newPassword { get; set; }
        public string email { get; set; }
    }
}