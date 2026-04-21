using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace ContactListAPI.Models
{
    /// <summary>
    /// Obiekt transferu danych reprezentujący pełne szczegóły kontaktu.
    /// Wykorzystywany przy operacjach tworzenia, edycji oraz pobierania szczegółowych informacji o pojedynczym kontakcie.
    /// </summary>
    public class ContactDetailsDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public required string Surname { get; set; }

        [Required(ErrorMessage ="Email jest wymagany.")]
        [EmailAddress(ErrorMessage ="Niepoprawny format adresu email.")]
        public required string Email { get; set; }

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
        ///<summary> 
        /// ID wybranej podkategorii (np. 'Szef'). Wypełniane tylko wtedy, gdy powyżej wybrano opcję 'Służbowy'. 
        ///</summary>
        public int? SubcategoryID { get; set; }

        ///<summary>
        /// Miejsce na tekst wpisany ręcznie, używane tylko wtedy, gdy użytkownik zaznaczył kategorię 'Inny'.
        ///</summary>
        public string? OwnSubcategory { get; set; }
        public string? CategoryName { get; set; }
        public string? SubcategoryName { get; set; }





    }
}
