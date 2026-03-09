using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers;

public class ReaderController : Controller
{
    private readonly IReaderRepository _readers;
    private readonly IBorrowingRepository _borrowings;
    private readonly IBookRepository _books;

    public ReaderController(IReaderRepository readers, IBorrowingRepository borrowings, IBookRepository books)
    {
        _readers = readers;
        _borrowings = borrowings;
        _books = books;
    }

    private bool IsLoggedIn => HttpContext.Session.GetString("Username") is not null;

    // GET: Reader
    public IActionResult Index(string? searchString)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");

        ViewData["CurrentFilter"] = searchString;
        var readers = _readers.GetAll();

        if (!string.IsNullOrWhiteSpace(searchString))
            readers = readers.Where(r =>
                r.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                r.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                (r.Phone != null && r.Phone.Contains(searchString, StringComparison.OrdinalIgnoreCase)));

        return View(readers.OrderBy(r => r.Name).ToList());
    }

    // GET: Reader/Details/5
    public IActionResult Details(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");

        var reader = _readers.GetById(id);
        if (reader is null) return NotFound();

        // Manually populate borrowing history for this reader
        var borrowings = _borrowings.GetAll().Where(b => b.ReaderId == id).ToList();
        foreach (var b in borrowings)
            b.Book ??= _books.GetById(b.BookId);
        reader.Borrowings = borrowings;

        return View(reader);
    }

    // GET: Reader/Create
    public IActionResult Create()
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        return View();
    }

    // POST: Reader/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Email,Phone,MemberSince")] Reader reader)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        if (!ModelState.IsValid) return View(reader);

        _readers.Add(reader);
        TempData["Success"] = $"Reader \"{reader.Name}\" added successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Reader/Edit/5
    public IActionResult Edit(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        var reader = _readers.GetById(id);
        if (reader is null) return NotFound();
        return View(reader);
    }

    // POST: Reader/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Name,Email,Phone,MemberSince")] Reader reader)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        if (id != reader.Id) return BadRequest();
        if (!ModelState.IsValid) return View(reader);

        _readers.Update(reader);
        TempData["Success"] = $"Reader \"{reader.Name}\" updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Reader/Delete/5
    public IActionResult Delete(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        var reader = _readers.GetById(id);
        if (reader is null) return NotFound();
        return View(reader);
    }

    // POST: Reader/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        var reader = _readers.GetById(id);
        if (reader is not null)
        {
            _readers.Delete(id);
            TempData["Success"] = $"Reader \"{reader.Name}\" deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }
}
