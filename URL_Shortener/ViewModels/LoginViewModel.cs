using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.ViewModels
{
    public class LoginViewModel
    {
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
