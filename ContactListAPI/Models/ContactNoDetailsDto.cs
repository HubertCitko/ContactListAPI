using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace ContactListAPI.Models
{
    /// <summary>
    /// Obiekt transferu danych zawierający podstawowe informacje o kontakcie.
    /// (nie zawiera danych wrażliwych).
    /// </summary>
    public class ContactNoDetailsDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public required string Surname { get; set; }

        [Required(ErrorMessage ="Email jest wymagany.")]
        [EmailAddress(ErrorMessage ="Niepoprawny format adresu email.")]
        public required string Email { get; set; }
        
    }
}
