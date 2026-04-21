using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace ContactListAPI.Models
{
    /// <summary>
    /// Klasa reprezentująca pojedynczy kontakt w bazie danych.
    /// Zawiera podstawowe dane użytkownika oraz relacje do słowników.
    /// </summary>
    [Index(nameof(Email), IsUnique = true)] // Kolumna email nie może mieć duplikatów
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public required string Surname { get; set; }

        [Required(ErrorMessage ="Email jest wymagany.")]
        [EmailAddress(ErrorMessage ="Niepoprawny format adresu email.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_-])[A-Za-z0-9!@#$%^&*()_-]{8,}$",
        ErrorMessage = "Hasło musi zawierać małą i dużą literę, cyfrę oraz znak specjalny oraz składać się z co najmniej 8 znaków.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Data urodzenia jest wymagana.")]
        public required DateTime BirthDate { get; set; }


        [Required(ErrorMessage = "Kategoria jest wymagana.")]
        ///<summary> 
        /// ID wybranej głównej kategorii z bazy danych (np. 1 dla 'Służbowy', 2 dla 'Prywatny') 
        ///</summary>
        public required int CategoryID { get; set; }
        public Category? Category { get; set; }
        ///<summary> 
        /// ID wybranej podkategorii (np. 'Szef'). Wypełniane tylko wtedy, gdy powyżej wybrano opcję 'Służbowy'. 
        ///</summary>
        public int? SubcategoryID { get; set; }
        public Subcategory? Subcategory { get; set; }

        ///<summary>
        /// Miejsce na tekst wpisany ręcznie, używane tylko wtedy, gdy użytkownik zaznaczył kategorię 'Inny'.
        ///</summary>
        public string? OwnSubcategory { get; set; }





    }
}
