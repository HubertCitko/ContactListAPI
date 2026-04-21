using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContactListAPI.Models
{
    /// <summary>
    /// Klasa reprezentująca słownik kategorie;
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; } 

        // Relacja: Jedna kategoria może mieć wiele podkategorii
        [JsonIgnore]
        public ICollection<Subcategory>? Subcategories { get; set; }
    }
}
