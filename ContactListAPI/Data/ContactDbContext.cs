using Microsoft.EntityFrameworkCore;
using ContactListAPI.Models;


namespace ContactListAPI.Data
{
    /// <summary>
    /// Główny kontekst bazy danych dla aplikacji.
     /// Odpowiada za mapowanie modeli na tabele w relacyjnej bazie danych oraz konfigurację początkową.
    /// </summary>
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
        {
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        /// <summary>
        /// Konfiguracja schematu bazy danych podczas jej tworzenia.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Służbowy" },
                new Category { Id = 2, Name = "Prywatny" },
                new Category { Id = 3, Name = "Inny" }
            );

            modelBuilder.Entity<Subcategory>().HasData(
                new Subcategory { Id = 1, Name = "Szef", CategoryId = 1 },
                new Subcategory { Id = 2, Name = "Klient", CategoryId = 1 },
                new Subcategory { Id = 3, Name = "Współpracownik", CategoryId = 1 }
            );
        }
    }
}
