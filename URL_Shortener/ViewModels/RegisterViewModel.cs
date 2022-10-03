using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.ViewModels
{
    public class RegisterViewModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Password don`t match")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        public string? PasswordConfirm { get; set; }

    }
}
