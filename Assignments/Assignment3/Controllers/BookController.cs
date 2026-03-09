using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers;

public class BookController : Controller
{
    private readonly IBookRepository _books;
    private readonly IBorrowingRepository _borrowings;
    private readonly IReaderRepository _readers;

    public BookController(IBookRepository books, IBorrowingRepository borrowings, IReaderRepository readers)
    {
        _books = books;
        _borrowings = borrowings;
        _readers = readers;
    }

    private bool IsLoggedIn => HttpContext.Session.GetString("Username") is not null;

    // GET: Book
    public IActionResult Index(string? searchString)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");

        ViewData["CurrentFilter"] = searchString;

        var books = _books.GetAll();

        if (!string.IsNullOrWhiteSpace(searchString))
            books = books.Where(b =>
                b.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                (b.ISBN != null && b.ISBN.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                (b.Genre != null && b.Genre.Contains(searchString, StringComparison.OrdinalIgnoreCase)));

        return View(books.OrderBy(b => b.Title).ToList());
    }

    // GET: Book/Details/5
    public IActionResult Details(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");

        var book = _books.GetById(id);
        if (book is null) return NotFound();

        // Manually populate borrowing history for this book
        var borrowings = _borrowings.GetAll().Where(b => b.BookId == id).ToList();
        foreach (var b in borrowings)
            b.Reader ??= _readers.GetById(b.ReaderId);
        book.Borrowings = borrowings;

        return View(book);
    }

    // GET: Book/Create
    public IActionResult Create()
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        return View();
    }

    // POST: Book/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title,Author,ISBN,Genre,PublishedYear,TotalCopies,AvailableCopies")] Book book)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        if (!ModelState.IsValid) return View(book);

        _books.Add(book);
        TempData["Success"] = $"Book \"{book.Title}\" added successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Book/Edit/5
    public IActionResult Edit(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        var book = _books.GetById(id);
        if (book is null) return NotFound();
        return View(book);
    }

    // POST: Book/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Title,Author,ISBN,Genre,PublishedYear,TotalCopies,AvailableCopies")] Book book)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        if (id != book.Id) return BadRequest();
        if (!ModelState.IsValid) return View(book);

        _books.Update(book);
        TempData["Success"] = $"Book \"{book.Title}\" updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Book/Delete/5
    public IActionResult Delete(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");

        var book = _books.GetById(id);
        if (book is null) return NotFound();
        return View(book);
    }

    // POST: Book/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        var book = _books.GetById(id);
        if (book is not null)
        {
            _books.Delete(id);
            TempData["Success"] = $"Book \"{book.Title}\" deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }
}
