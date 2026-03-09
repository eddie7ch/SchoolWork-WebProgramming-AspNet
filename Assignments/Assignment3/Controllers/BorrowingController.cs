using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS.Controllers;

public class BorrowingController : Controller
{
    private readonly IBorrowingRepository _borrowings;
    private readonly IBookRepository _books;
    private readonly IReaderRepository _readers;

    public BorrowingController(IBorrowingRepository borrowings, IBookRepository books, IReaderRepository readers)
    {
        _borrowings = borrowings;
        _books = books;
        _readers = readers;
    }

    private bool IsLoggedIn => HttpContext.Session.GetString("Username") is not null;

    private void PopulateDropdowns(int? selectedBookId = null, int? selectedReaderId = null)
    {
        var books = _books.GetAll()
            .OrderBy(b => b.Title)
            .Select(b => new { b.Id, Display = $"{b.Title} ({b.AvailableCopies} available)" })
            .ToList();
        ViewBag.BookId   = new SelectList(books, "Id", "Display", selectedBookId);
        ViewBag.ReaderId = new SelectList(_readers.GetAll().OrderBy(r => r.Name), "Id", "Name", selectedReaderId);
    }

    private void ResolveNavProperties(IEnumerable<Borrowing> borrowings)
    {
        foreach (var b in borrowings)
        {
            b.Book   ??= _books.GetById(b.BookId);
            b.Reader ??= _readers.GetById(b.ReaderId);
        }
    }

    // GET: Borrowing
    public IActionResult Index(string? searchString)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");

        ViewData["CurrentFilter"] = searchString;
        var borrowings = _borrowings.GetAll().ToList();
        ResolveNavProperties(borrowings);

        if (!string.IsNullOrWhiteSpace(searchString))
            borrowings = borrowings.Where(b =>
                (b.Book?.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true) ||
                (b.Reader?.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true) ||
                b.Status.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();

        return View(borrowings.OrderByDescending(b => b.BorrowDate).ToList());
    }

    // GET: Borrowing/Details/5
    public IActionResult Details(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        var borrowing = _borrowings.GetById(id);
        if (borrowing is null) return NotFound();
        borrowing.Book   ??= _books.GetById(borrowing.BookId);
        borrowing.Reader ??= _readers.GetById(borrowing.ReaderId);
        return View(borrowing);
    }

    // GET: Borrowing/Create
    public IActionResult Create()
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        PopulateDropdowns();
        return View(new Borrowing { BorrowDate = DateTime.Today, DueDate = DateTime.Today.AddDays(14) });
    }

    // POST: Borrowing/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("BookId,ReaderId,BorrowDate,DueDate")] Borrowing borrowing)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");

        ModelState.Remove(nameof(Borrowing.Book));
        ModelState.Remove(nameof(Borrowing.Reader));

        if (!ModelState.IsValid)
        {
            PopulateDropdowns(borrowing.BookId, borrowing.ReaderId);
            return View(borrowing);
        }

        var book = _books.GetById(borrowing.BookId);
        if (book is null || book.AvailableCopies < 1)
        {
            ModelState.AddModelError("BookId", "The selected book has no available copies.");
            PopulateDropdowns(borrowing.BookId, borrowing.ReaderId);
            return View(borrowing);
        }

        book.AvailableCopies--;
        borrowing.IsReturned = false;
        borrowing.Status = "Active";
        _borrowings.Add(borrowing);
        TempData["Success"] = "Borrowing record created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Borrowing/Edit/5
    public IActionResult Edit(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        var borrowing = _borrowings.GetById(id);
        if (borrowing is null) return NotFound();
        PopulateDropdowns(borrowing.BookId, borrowing.ReaderId);
        return View(borrowing);
    }

    // POST: Borrowing/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,BookId,ReaderId,BorrowDate,DueDate,ReturnDate,IsReturned,Status")] Borrowing borrowing)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        if (id != borrowing.Id) return BadRequest();

        ModelState.Remove(nameof(Borrowing.Book));
        ModelState.Remove(nameof(Borrowing.Reader));

        if (!ModelState.IsValid)
        {
            PopulateDropdowns(borrowing.BookId, borrowing.ReaderId);
            return View(borrowing);
        }

        var existing = _borrowings.GetById(id);
        if (existing is null) return NotFound();

        if (borrowing.IsReturned && !existing.IsReturned)
        {
            var book = _books.GetById(borrowing.BookId);
            if (book is not null) book.AvailableCopies++;
            if (borrowing.ReturnDate is null) borrowing.ReturnDate = DateTime.Today;
            borrowing.Status = "Returned";
        }
        else if (!borrowing.IsReturned)
        {
            borrowing.Status = borrowing.DueDate < DateTime.Today ? "Overdue" : "Active";
        }

        _borrowings.Update(borrowing);
        TempData["Success"] = "Borrowing record updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Borrowing/Delete/5
    public IActionResult Delete(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        var borrowing = _borrowings.GetById(id);
        if (borrowing is null) return NotFound();
        borrowing.Book   ??= _books.GetById(borrowing.BookId);
        borrowing.Reader ??= _readers.GetById(borrowing.ReaderId);
        return View(borrowing);
    }

    // POST: Borrowing/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");
        var borrowing = _borrowings.GetById(id);
        if (borrowing is not null)
        {
            if (!borrowing.IsReturned)
            {
                var book = _books.GetById(borrowing.BookId);
                if (book is not null) book.AvailableCopies++;
            }
            _borrowings.Delete(id);
            TempData["Success"] = "Borrowing record deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }
}
