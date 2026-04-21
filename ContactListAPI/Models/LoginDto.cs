using System.ComponentModel.DataAnnotations;
namespace ContactListAPI.Models
{
    /// <summary>
    /// Obiekt DTO używany wyłącznie do odbierania danych logowania z frontendu.
    /// </summary>
    public class LoginDto
    {

        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        public required string Password { get; set; }
    }
}
