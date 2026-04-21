using ContactListAPI.Data;
using ContactListAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Kontroler zarządzający operacjami na kontaktach.
    /// Udostępnia endpointy REST API do pobierania, dodawania, edytowania i usuwania kontaktów.
    /// </summary>
    public class ContactController : ControllerBase
    {
        private readonly ContactDbContext _context;

        public ContactController(ContactDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Pobiera listę wszystkich kontaktów.
        /// </summary>
        /// <returns>Lista podstawowych danych kontaktów.</returns>
        [AllowAnonymous]
        [HttpGet]

        public async Task<ActionResult<IEnumerable<ContactNoDetailsDto>>> GetContacts()
        {
            // Wyciągnij wszystkie kontakty z bazy danych i zrzutuj je na objekt
            var contacts = await _context.Contacts
                .Select(c => new ContactNoDetailsDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Surname = c.Surname,
                    Email = c.Email,
                })
                .ToListAsync();

            return contacts;
        }
        /// <summary>
        /// Pobiera szczegółowe dane konkretnego kontaktu na podstawie jego identyfikatora ID.
        /// </summary>
        /// <param name="id"> Identyfikator kontaktu.</param>
        /// <returns>Szczegóły wybranego kontaktu lub status 404 (NotFound).</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]

        public async Task<ActionResult<ContactDetailsDto>> GetContact(int id)
        {
            var contact = await _context.Contacts
            .Include(c => c.Category)
            .Include(c => c.Subcategory)
            .FirstOrDefaultAsync(c => c.Id == id);

            if(contact == null)
            {
                return NotFound("Nie znaleziono takiego kontaktu.");
            }

            // Rzutujemy dane nie podajac hasha
            return new ContactDetailsDto {
                Id = contact.Id,
                Name = contact.Name,
                Surname = contact.Surname,
                Email = contact.Email,
                Phone = contact.Phone,
                BirthDate = contact.BirthDate,
                CategoryID = contact.CategoryID,
                SubcategoryID = (contact.CategoryID == 1) ? contact.SubcategoryID : null,
                OwnSubcategory = (contact.CategoryID == 3) ? contact.OwnSubcategory : null,
                CategoryName = contact.Category?.Name,
                SubcategoryName = contact.Subcategory?.Name
            };
        }

        /// <summary>
        /// Aktualizuje istniejący wpis kontaktu. Wymaga autoryzacji (zalogowanego użytkownika).
        /// </summary>
        /// <param name="id">Identyfikator modyfikowanego kontaktu.</param>
        /// <param name="contact">Zaktualizowany obiekt kontaktu.</param>
        /// <returns>Status 204 (No content) jeśli operacja się powiedzie.</returns>
        
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, ContactDetailsDto contact)
        {
            // Sprawdz czy zgadza sie id
            if (id != contact.Id) return BadRequest($"Niezgodność ID: URL ma {id}, a obiekt {contact.Id}");

            // Pobierz kontakt
            var contactInDb = await _context.Contacts.FindAsync(id);
            if (contactInDb == null)
            {
                return NotFound("Nie znaleziono kontaktu o podanym ID.");
            }

            // Sprawdz czy email nie znajduje sie już w bazie danych
            bool emailExists = await _context.Contacts.AnyAsync(c => c.Email == contact.Email && c.Id != id);
            if (emailExists)
            {
                return BadRequest("Ten email jest już zajęty.");
            }
            // Walidacja kategorii
            if (contact.CategoryID == 1 && contact.SubcategoryID == null)
            {
                return BadRequest("Dla kategorii 'Służbowy' podkategoria jest wymagana.");
            }
            if (contact.CategoryID == 3 && string.IsNullOrWhiteSpace(contact.OwnSubcategory))
            {
                return BadRequest("Dla kategorii 'Inny' musisz wpisać własną nazwę podkategorii.");
            }

            // Modyfikacja kontaktu
            contactInDb.Id = contact.Id;
            contactInDb.Name = contact.Name;
            contactInDb.Surname = contact.Surname;
            contactInDb.Email = contact.Email;
            contactInDb.Phone = contact.Phone;
            contactInDb.BirthDate = contact.BirthDate;
            contactInDb.CategoryID = contact.CategoryID;
            contactInDb.SubcategoryID = (contact.CategoryID == 1) ? contact.SubcategoryID : null;
            contactInDb.OwnSubcategory = (contact.CategoryID == 3) ? contact.OwnSubcategory : null;
            
            try
            {
                // Zaktualizuj baze
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Nie udało się zapisać kontaktu

                // Sprawdz czy powodem jest brak kontaktu w bazie danych
                if (!await _context.Contacts.AnyAsync(e => e.Id == id)) return NotFound();
                // W przeciwnym wypadku rzuć błąd dalej
                else throw;
            }
            return NoContent();
        }
        /// <summary>
        /// Usuwa wybrany kontakt z bazy danych.
        /// </summary>
        /// <param name="id">Identyfikator kontaktu do usunięcia.</param>
        /// <returns>Status 204 (No Content) w przypadku powodzenia.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            //Znajdz kontakt po id
            var contact = await _context.Contacts.FindAsync(id);
            // Kontakt nie istnieje
            if (contact == null) return NotFound();

            //Usun i zaktualizuj
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        /// <summary>
        /// Tworzy nowy kontakt w bazie danych.
        /// </summary>
        /// <param name="contact">Obiekt kontaktu do dodania.</param>
        /// <returns>Zwraca status 201 (Created) oraz dodany kontakt.</returns>
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            // Sprawdz czy email nie znajduje sie już w bazie danych
            bool emailExists = await _context.Contacts.AnyAsync(c => c.Email == contact.Email);
            if(emailExists)
            {
                return BadRequest("Ten email jest już zajęty.");
            }

            // Walidacja kategorii
            if (contact.CategoryID == 1) // 1 = Służbowy
            {
                if (contact.SubcategoryID == null)
                    return BadRequest("Dla kategorii 'Służbowy' musisz wybrać podkategorię ze słownika.");

                contact.OwnSubcategory = null; // Czyścimy tekst, żeby zachować spójność w bazie
            }
            else if (contact.CategoryID == 2) // 2 = Prywatny bez podkategorii
            {
                contact.SubcategoryID = null;
                contact.OwnSubcategory = null; 
            }
            else if (contact.CategoryID == 3) // 3 = Inny
            {
                if (string.IsNullOrWhiteSpace(contact.OwnSubcategory))
                    return BadRequest("Dla kategorii 'Inny' musisz wpisać własną podkategorię.");
                contact.SubcategoryID = null; // Czyścimy ID podkategorii
            }
            else
            {
                return BadRequest("Wybrano nieprawidłową kategorię.");
            }
            // Zmień jawne hasło na zhashowane
            contact.Password = BCrypt.Net.BCrypt.HashPassword(contact.Password);

            // Dodaj kontakt do bazy danych
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            // Zwróć kod 201 oraz link do nowo stworzonego obiektu.
            return CreatedAtAction(nameof(GetContacts), new { id = contact.Id }, contact);
        }
    }
}
