using System.ComponentModel.DataAnnotations;

namespace Esport.Shared.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Podaj adres e-mail lub login.")]
        public string Identifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj hasło.")]
        [MinLength(6, ErrorMessage = "Hasło musi mieć przynajmniej 6 znaków.")]
        public string Password { get; set; } = string.Empty;
    }
}
