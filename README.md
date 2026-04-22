# System Zarządzania Listą Kontaktów

## Opis Projektu
Aplikacja została stworzona jako rozwiązanie zadania rekrutacyjnego. Składa się z części serwerowej (Web API) oraz klienckiej (Blazor WebAssembly), umożliwiając zarządzanie listą kontaktów z podziałem na kategorie i podkategorie. System zapewnia bezpieczeństwo danych dzięki uwierzytelnianiu oprogramowanemu za pomocą tokenów JWT.

## Wykorzystane Biblioteki

### Backend (API)
- **Microsoft.EntityFrameworkCore.SqlServer**: ORM do obsługi bazy danych SQL Server.
- **BCrypt.Net-Next**: Biblioteka do bezpiecznego hashowania haseł użytkowników.
- **Microsoft.AspNetCore.Authentication.JwtBearer**: Obsługa autoryzacji opartej na tokenach JWT.
- **Scalar.AspNetCore**: Generowanie interaktywnej dokumentacji OpenAPI.

### Frontend (SPA)
- **Blazored.LocalStorage**: Biblioteka do zarządzania lokalnym magazynem przeglądarki (przechowywanie tokena sesji).
- **Microsoft.AspNetCore.Components.WebAssembly**: Framework do budowy aplikacji typu Single Page Application.

## Klasy i metody

### Backend (Projekt ContactListAPI)
- **AuthController**: Zarządza procesem uwierzytelniania.
    - `Login`: Metoda weryfikująca poświadczenia użytkownika i generująca podpisany token JWT.
- **ContactController**: Implementuje pełną logikę CRUD dla kontaktów.
    - `GetContacts`: Zwraca listę podstawowych danych kontaktów (dostęp publiczny).
    - `GetContact`: Zwraca szczegółowe dane pojedynczego kontaktu wraz z nazwami kategorii (dostęp publiczny).
    - `PostContact`: Dodaje nowy kontakt, wykonuje walidację logiki kategorii i hashuje hasło.
    - `PutContact`: Bezpiecznie aktualizuje dane kontaktu przy użyciu obiektów DTO.
    - `DeleteContact`: Usuwa wybrany kontakt z bazy.
- **Data (ContactDbContext)**: Kontekst bazy danych zaimplementowany w standardzie Code-First. Zawiera logikę *Data Seeding* dla kategorii (Służbowy, Prywatny, Inny) oraz podkategorii.
- **Modele i DTO**: Rozdzielono encje bazodanowe (`Contact`) od obiektów transferu danych (`ContactDetailsDto`, `LoginDto`) po to aby przesyłać tylko niezbędne dnae

### Frontend (Projekt ContactListFrontend)
- **MainLayout.razor**: Zarządza globalnym stanem logowania za pomocą parametru kaskadowego.
- **Home.razor**: Wyświetla tabelę kontaktów i obsługuje usuwanie rekordów z potwierdzeniem użytkownika.
- **NewContact.razor / EditContact.razor**: Komponenty obsługujące formularze z dynamiczną zmianą pól w zależności od wybranej kategorii.
- **viewContact.razor**: Strona podglądu szczegółów kontaktu dostępna dla wszystkich użytkowników.

### Instrukcja Kompilacji Projektu

### Krok 1: Wymagania
Upewnij się, że na komputerze zainstalowany jest **.NET 9.06 SDK**.

### Krok 2: Przygotowanie Bazy Danych
1.  W pliku `appsettings.json` projektu **ContactListAPI** ustaw poprawne połączenie w `DefaultConnection` (domyślnie: `(localdb)\\mssqllocaldb`).
2.  Zainstaluj narzędzia Entity Framework (jeśli nie są zainstalowane):
    `dotnet tool install --global dotnet-ef`
3.  Uruchom migrację bazy danych (będąc w folderze projektu API):
    `dotnet restore`
    `dotnet ef database update`
    *(Operacja utworzy tabele i zasili słowniki danych).*

#### Opcja A: Visual Studio
1.  Kliknij Prawym Przyciskiem Myszy na **Solution** -> **Properties**.
2.  Wybierz **Multiple startup projects**.
3.  Dla projektów `ContactListAPI` oraz `ContactListFrontend` ustaw akcję na **Start** i zapisz zmiany.
4.  Uruchom system przyciskiem **Start (F5)**.

#### Opcja B: Terminal
Wymagane jest uruchomienie dwóch procesów w osobnych terminalach:
1.  **Terminal 1 (Backend):** Wejdź do folderu `ContactListAPI` i wpisz `dotnet run --urls "https://localhost:7024"`.
2.  **Terminal 2 (Frontend):** Wejdź do folderu `ContactListFrontend` i wpisz `dotnet run`.

Po uruchomieniu aplikacja otworzy się w przeglądarce. Interaktywna dokumentacja techniczna API dostępna jest pod adresem: `https://localhost:7024/scalar/`.

**Login i hasło do konta**
 email : admin@gmail.com
 haslo : Admin111!!!
