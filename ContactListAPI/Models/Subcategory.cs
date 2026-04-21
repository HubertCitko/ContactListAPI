using System.Text.Json.Serialization;

namespace ContactListAPI.Models
{
    /// <summary>
    /// Klasa reprezentująca słownik podkategorii;
    /// </summary>
    public class Subcategory
    {
        public int Id { get; set; }
        public required string Name { get; set; } 

        // Relacja (Klucz obcy do kategorii)
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
