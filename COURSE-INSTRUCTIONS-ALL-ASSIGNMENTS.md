# Web Programming (ASP.NET) — All Assignments & Course Context
**Course:** Web Programming — ASP.NET | Bow Valley College
**Student:** Eddie Chongtham (eddie7ch)
**Repo:** SchoolWork-WebProgramming-ASPNET (Public on GitHub)
**Submission filename format:** `chongtham_eddie_[assignment]_#`

---

## Course Overview

This course covers full-stack web development with ASP.NET (.NET 9):
- Minimal API (MapGet, MapPost, MapPut, MapDelete)
- MVC architecture (Controllers, Models, Views)
- Repository pattern (data access abstraction)
- Entity Framework Core + migrations + Azure SQL
- Razor Views + Bootstrap 5
- Session-based authentication (SHA-256 hashing)
- Azure App Service deployment

---

## Repository Structure

```
SchoolWork-WebProgramming-ASPNET/
├── Activities/         ← in-class and lab exercises
├── Assignments/
│   ├── Assignment1/    ← Minimal API (no DB)
│   ├── Assignment2/    ← MVC API with Controllers/Models
│   ├── Assignment3/    ← Full MVC with Views, Auth, Repository pattern
│   └── Assignment4/    ← EF Core + migrations + database
└── Projects/
    ├── VehicleRental/  ← Group project (Azure deployed)
    └── SVG files/      ← Diagram exports
```

---

## Assignment 1 — Minimal API (Library System)

**Folder:** `Assignments/Assignment1/`
**Focus:** ASP.NET Core Minimal API — route mapping, HTTP verbs, JSON responses

### What was built
A RESTful Minimal API for a Library system without a database. All data is hardcoded in `Program.cs`. Three resource types:
1. **Books** — `id`, `title`, `author`, `available`
2. **Readers** — `id`, `name`, `email`, `phone`
3. **Borrowings** — `id`, `bookId`, `readerId`, `borrowDate`, `dueDate`, `returnDate`, `status`

### All Endpoints

**Books:**
```
GET    /api/books          → Returns list of 3 books
GET    /api/books/{id}     → Returns single book by id
POST   /api/books          → Returns Created (201) with new book
PUT    /api/books/{id}     → Returns updated book
DELETE /api/books/{id}     → Returns deletion confirmation
```

**Readers:**
```
GET    /api/readers          → Returns list of 2 readers
GET    /api/readers/{id}     → Returns single reader by id
POST   /api/readers          → Returns Created (201) with new reader
PUT    /api/readers/{id}     → Returns updated reader
DELETE /api/readers/{id}     → Returns deletion confirmation
```

**Borrowings:**
```
GET    /api/borrowings          → Returns list of 2 borrowings
GET    /api/borrowings/{id}     → Returns single borrowing
POST   /api/borrowings          → Returns Created (201)
PUT    /api/borrowings/{id}     → Returns updated borrowing (e.g. return a book)
DELETE /api/borrowings/{id}     → Cancels a borrowing
```

### Folder Structure
```
Assignment1/
├── Program.cs          ← All routes hardcoded here
├── Assignment1.csproj
└── Assignment1.http    ← HTTP test file
```

### Key Patterns
```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/api/books", () => Results.Ok(new { message = "...", books = [...] }));
app.MapPost("/api/books", () => Results.Created("/api/books/4", new { ... }));
app.MapPut("/api/books/{id:int}", (int id) => Results.Ok(new { ... }));
app.MapDelete("/api/books/{id:int}", (int id) => Results.Ok(new { ... }));
app.Run();
```

---

## Assignment 2 — MVC API with Controllers and Models

**Folder:** `Assignments/Assignment2/`
**Focus:** Proper MVC structure — Controllers, strongly-typed Model classes

### What was built
Same Library system as Assignment 1 but refactored into proper MVC with separate model classes:
- `Book.cs` — Book model
- `Reader.cs` — Reader model
- `Borrowing.cs` — Borrowing model
- Three API controllers: `BookController`, `ReaderController`, `BorrowingController`

### Folder Structure
```
Assignment2/
├── Controllers/
│   ├── BookController.cs
│   ├── BorrowingController.cs
│   └── ReaderController.cs
├── Models/
│   ├── Book.cs
│   ├── Borrowing.cs
│   └── Reader.cs
├── Program.cs
└── Assignment2.csproj
```

### Controller Pattern
```csharp
[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() { ... }

    [HttpGet("{id}")]
    public IActionResult GetById(int id) { ... }

    [HttpPost]
    public IActionResult Create([FromBody] Book book) { ... }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Book book) { ... }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) { ... }
}
```

---

## Assignment 3 — Full MVC Web App with Views, Auth, and Repository Pattern

**Folder:** `Assignments/Assignment3/`
**Focus:** Full MVC web app — Razor Views, Authentication, Repository pattern, Reports

### What was built
A full web application (not just an API) with UI pages for the Library system:
- **Authentication:** Login and Register pages with `AccountController`
- **User model:** `User.cs` with login/register ViewModels
- **ViewModels:** `LoginViewModel`, `RegisterViewModel`, `ErrorViewModel`
- **Repository pattern:** Data access abstracted behind repository interfaces
- **Reports section:** `ReportController` for usage/borrowing reports
- **Full Razor Views** for all controllers + shared layouts

### Folder Structure
```
Assignment3/
├── Controllers/
│   ├── AccountController.cs    ← Login, Register, Logout
│   ├── BookController.cs
│   ├── BorrowingController.cs
│   ├── HomeController.cs
│   ├── ReaderController.cs
│   └── ReportController.cs
├── Models/
│   ├── Book.cs
│   ├── Borrowing.cs
│   ├── User.cs
│   ├── LoginViewModel.cs
│   ├── RegisterViewModel.cs
│   └── ErrorViewModel.cs
├── Repositories/               ← Repository pattern (data access layer)
├── Views/                      ← Razor Views for each controller
├── wwwroot/                    ← Static files (CSS, JS, Bootstrap, jQuery)
└── Program.cs
```

### Key Patterns
```csharp
// Repository pattern
public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book GetById(int id);
    void Add(Book book);
    void Update(Book book);
    void Delete(int id);
}

// Session-based auth in AccountController
HttpContext.Session.SetString("UserId", user.Id.ToString());
var userId = HttpContext.Session.GetString("UserId");
```

---

## Assignment 4 — Entity Framework Core + Database + Migrations

**Folder:** `Assignments/Assignment4/`
**Focus:** EF Core, Code-First database, migrations, full CRUD with real persistence

### What was built
Same Library system but now with a real database using Entity Framework Core:
- `Data/` folder with `DbContext` class
- `Migrations/` folder with EF migration files
- All CRUD operations now persist to SQL Server LocalDB
- No Repositories folder — data access goes through DbContext directly
- Authentication is now session-based (SHA-256 hashing)

### Folder Structure
```
Assignment4/
├── Controllers/
│   ├── BookController.cs
│   ├── BorrowingController.cs
│   ├── HomeController.cs
│   └── ReaderController.cs
├── Data/                   ← DbContext
├── Migrations/             ← EF generated migration files
├── Models/
│   ├── Book.cs
│   ├── Borrowing.cs
│   ├── Reader.cs
│   └── ErrorViewModel.cs
├── Views/
├── wwwroot/
└── Program.cs
```

### Key EF Core Patterns
```csharp
// DbContext
public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }
}

// Inject in controller
public class BookController : Controller
{
    private readonly AppDbContext _context;
    public BookController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Books.ToListAsync());
}

// Migrations
// dotnet ef migrations add InitialCreate
// dotnet ef database update
```

---

## Project — Vehicle Rental Management System (Group Project)

**Folder:** `Projects/VehicleRental/`
**Repo:** https://github.com/bvc-g9-vehicle-rental/Vehicle-Rental-Management-System
**Live:** https://vehicle-rental-app.azurewebsites.net
**Team:** Eddie Chongtham (eddie7ch) + Zoubera Issaka

### What was built
A full web-based vehicle rental management system:

**Features:**
1. Login & Registration — session-based auth with SHA-256 password hashing
2. Dashboard — fleet overview, reservation stats, customer count, revenue
3. Vehicle Management — add, edit, delete vehicles; track availability status
4. Customer Management — full CRUD customer database
5. Reservation Management — create, modify, cancel reservations
6. Billing — auto-calculate rental cost, tax, and additional charges
7. Reports — reservation history, revenue summary, vehicle availability

**Default admin credentials:**
- Username: `admin`
- Password: `Admin123!`

### Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET MVC (.NET 9) |
| Database | Entity Framework Core + Azure SQL |
| Auth | Custom session-based (SHA-256) |
| UI | Razor Views + Bootstrap 5 |
| Hosting | Azure App Service (Free F1) |

### Folder Structure
```
VehicleRental/
├── Controllers/     ← Account, Home, Vehicle, Customer, Reservation, Billing, Report
├── Models/          ← EF Core models + ViewModels
├── Views/           ← Razor Views
├── Repositories/    ← Repository pattern (data access layer)
├── Data/            ← DbContext + DbSeeder
├── Migrations/      ← EF Core migrations
└── wwwroot/         ← Static files (CSS, JS)
```

### Design Documents
- `ER-Diagram.drawio` / `.png` / `.svg` — Entity Relationship diagram
- `UI-Wireframes.drawio` / `.png` / `.svg` — UI wireframes
- `Project-Design.docx` — full project design document
- `CONTRIBUTING.md` — development workflow (branches: main, eddie-dev, zoubera-dev)

### Run Locally
```bash
cd Projects/VehicleRental
dotnet run
# Requires .NET 9 SDK and SQL Server LocalDB
```

---

## Build & Run Summary

```bash
# Assignment 1 (Minimal API)
cd Assignments/Assignment1
dotnet run
# Test endpoints in Assignment1.http or with curl

# Assignment 2 (MVC API)
cd Assignments/Assignment2
dotnet run

# Assignment 3 (Full MVC Web App)
cd Assignments/Assignment3
dotnet run
# Open browser at http://localhost:5XXX

# Assignment 4 (EF Core)
cd Assignments/Assignment4
dotnet ef database update   # first time only
dotnet run

# VehicleRental Project
cd Projects/VehicleRental
dotnet run
# Or visit: https://vehicle-rental-app.azurewebsites.net
```
