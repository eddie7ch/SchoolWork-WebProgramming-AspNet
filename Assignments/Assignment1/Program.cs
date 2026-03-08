var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

// ─────────────────────────────────────────────
// BOOKS
// ─────────────────────────────────────────────

// GET all books
app.MapGet("/api/books", () =>
{
    return Results.Ok(new
    {
        message = "Retrieved all books",
        books = new[]
        {
            new { id = 1, title = "The Great Gatsby",  author = "F. Scott Fitzgerald", available = true  },
            new { id = 2, title = "To Kill a Mockingbird", author = "Harper Lee",        available = false },
            new { id = 3, title = "1984",               author = "George Orwell",        available = true  }
        }
    });
});

// GET a single book by id
app.MapGet("/api/books/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        message = $"Retrieved book with ID {id}",
        book = new { id, title = "Sample Book", author = "Sample Author", available = true }
    });
});

// POST create a new book
app.MapPost("/api/books", () =>
{
    return Results.Created("/api/books/4", new
    {
        message = "Book created successfully",
        book = new { id = 4, title = "New Book Title", author = "New Author", available = true }
    });
});

// PUT update an existing book
app.MapPut("/api/books/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        message = $"Book with ID {id} updated successfully",
        book = new { id, title = "Updated Book Title", author = "Updated Author", available = false }
    });
});

// DELETE a book
app.MapDelete("/api/books/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        message = $"Book with ID {id} deleted successfully"
    });
});

// ─────────────────────────────────────────────
// READERS
// ─────────────────────────────────────────────

// GET all readers
app.MapGet("/api/readers", () =>
{
    return Results.Ok(new
    {
        message = "Retrieved all readers",
        readers = new[]
        {
            new { id = 1, name = "Alice Johnson", email = "alice@example.com", phone = "403-111-2222" },
            new { id = 2, name = "Bob Smith",     email = "bob@example.com",   phone = "403-333-4444" }
        }
    });
});

// GET a single reader by id
app.MapGet("/api/readers/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        message = $"Retrieved reader with ID {id}",
        reader = new { id, name = "Sample Reader", email = "reader@example.com", phone = "403-000-0000" }
    });
});

// POST create a new reader
app.MapPost("/api/readers", () =>
{
    return Results.Created("/api/readers/3", new
    {
        message = "Reader created successfully",
        reader = new { id = 3, name = "New Reader", email = "newreader@example.com", phone = "403-555-6666" }
    });
});

// PUT update an existing reader
app.MapPut("/api/readers/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        message = $"Reader with ID {id} updated successfully",
        reader = new { id, name = "Updated Reader", email = "updated@example.com", phone = "403-777-8888" }
    });
});

// DELETE a reader
app.MapDelete("/api/readers/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        message = $"Reader with ID {id} deleted successfully"
    });
});

// ─────────────────────────────────────────────
// BORROWINGS
// ─────────────────────────────────────────────

// GET all borrowings
app.MapGet("/api/borrowings", () =>
{
    return Results.Ok(new
    {
        message = "Retrieved all borrowings",
        borrowings = new[]
        {
            new { id = 1, bookId = 2, readerId = 1, borrowDate = "2026-02-01", dueDate = "2026-02-15", returnDate = (string?)null,         status = "Active"   },
            new { id = 2, bookId = 1, readerId = 2, borrowDate = "2026-01-10", dueDate = "2026-01-24", returnDate = (string?)"2026-01-22", status = "Returned" }
        }
    });
});

// GET a single borrowing by id
app.MapGet("/api/borrowings/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        message = $"Retrieved borrowing with ID {id}",
        borrowing = new { id, bookId = 1, readerId = 1, borrowDate = "2026-03-01", dueDate = "2026-03-15", returnDate = (string?)null, status = "Active" }
    });
});

// POST create a new borrowing
app.MapPost("/api/borrowings", () =>
{
    return Results.Created("/api/borrowings/3", new
    {
        message = "Borrowing created successfully",
        borrowing = new { id = 3, bookId = 3, readerId = 2, borrowDate = "2026-03-08", dueDate = "2026-03-22", returnDate = (string?)null, status = "Active" }
    });
});

// PUT update a borrowing (e.g. return a book or modify due date)
app.MapPut("/api/borrowings/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        message = $"Borrowing with ID {id} updated successfully",
        borrowing = new { id, bookId = 2, readerId = 1, borrowDate = "2026-02-01", dueDate = "2026-02-20", returnDate = (string?)"2026-03-08", status = "Returned" }
    });
});

// DELETE (cancel) a borrowing
app.MapDelete("/api/borrowings/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        message = $"Borrowing with ID {id} cancelled successfully"
    });
});

app.Run();

