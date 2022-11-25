using System.ComponentModel.DataAnnotations;

namespace iTravelerServer.Domain.ViewModels.AccountVM
{
    public class LoginRequestVM
    {
        // [Required(ErrorMessage = "Введите email")]
        // [MaxLength(50, ErrorMessage = "email должен иметь длину меньше 50 символов")]
        // [MinLength(5, ErrorMessage = "email должен иметь длину больше 3 символов")]
        //public string Email { get; set; }

        // [Required(ErrorMessage = "Введите пароль")]
        // [DataType(DataType.Password)]
        // [Display(Name = "Пароль")]
        public string Password { get; set; }
        public string Email { get; set; }
    }
    
    
}   