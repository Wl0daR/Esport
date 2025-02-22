using System.ComponentModel.DataAnnotations;

namespace Esport.Shared.DTO
{
    public class RegisterModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Podaj adres e-mail.")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Podaj prawidłowy adres e-mail.")]
        public string Email { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Podaj login.")]
        public string Login { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Podaj hasło.")]
        [System.ComponentModel.DataAnnotations.MinLength(6, ErrorMessage = "Hasło musi mieć przynajmniej 6 znaków.")]
        public string Password { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Potwierdź hasło.")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Hasła muszą być zgodne.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

