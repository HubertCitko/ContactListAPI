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
    - `GetContact`: Zwraca szczegółowe dane pojedynczego kontaktu wraz z nazwami kategorii.
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

## Sposób Kompilacji i Uruchomienia

1.  **Wymagania**: .NET 8 SDK, SQL Server (np. LocalDB).
2.  **Konfiguracja**: Sprawdź `appsettings.json` w projekcie API i dostosuj `DefaultConnection`.
3.  **Baza Danych**: W folderze projektu API wykonaj komendę:
    ```bash
    dotnet ef database update
    ```
    Komenda utworzy tabele i zasili słowniki danych.
4.  **Uruchomienie**:
    - Uruchom projekt **ContactListAPI** (`dotnet run`).
    - Uruchom projekt **ContactListFrontend** (`dotnet run`).
5.  **Dokumentacja API**: Po uruchomieniu API, interaktywna dokumentacja dostępna jest pod adresem: `https://localhost:[port]/scalar/`.
"""