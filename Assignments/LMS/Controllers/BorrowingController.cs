using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS.Controllers;

public class BorrowingController : Controller
{
    private readonly IBorrowingRepository _borrowingRepo;
    private readonly IBookRepository      _bookRepo;
    private readonly IReaderRepository    _readerRepo;

    public BorrowingController(
        IBorrowingRepository borrowingRepo,
        IBookRepository bookRepo,
        IReaderRepository readerRepo)
    {
        _borrowingRepo = borrowingRepo;
        _bookRepo      = bookRepo;
        _readerRepo    = readerRepo;
    }

    // Resolve navigation properties for display
    private Borrowing Populate(Borrowing b)
    {
        b.Book   = _bookRepo.GetById(b.BookId);
        b.Reader = _readerRepo.GetById(b.ReaderId);
        return b;
    }

    // Populate ViewBag dropdowns
    private void PopulateDropdowns(int? selectedBookId = null, int? selectedReaderId = null)
    {
        var availableBooks = _bookRepo.GetAll().Where(b => b.IsAvailable).ToList();
        ViewBag.BookId   = new SelectList(availableBooks, "Id", "Title", selectedBookId);
        ViewBag.ReaderId = new SelectList(_readerRepo.GetAll(), "Id", "Name", selectedReaderId);
    }

    // GET: Borrowing
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var borrowings = _borrowingRepo.GetAll().Select(Populate).ToList();
        return View(borrowings);
    }

    // GET: Borrowing/Details/5
    public IActionResult Details(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var borrowing = _borrowingRepo.GetById(id);
        if (borrowing is null) return NotFound();
        return View(Populate(borrowing));
    }

    // GET: Borrowing/Create
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        PopulateDropdowns();
        return View(new Borrowing { BorrowDate = DateTime.Today, DueDate = DateTime.Today.AddDays(14) });
    }

    // POST: Borrowing/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Borrowing borrowing)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        // Remove navigation property validation errors
        ModelState.Remove(nameof(Borrowing.Book));
        ModelState.Remove(nameof(Borrowing.Reader));

        if (!ModelState.IsValid)
        {
            PopulateDropdowns(borrowing.BookId, borrowing.ReaderId);
            return View(borrowing);
        }

        var book = _bookRepo.GetById(borrowing.BookId);
        if (book is null || !book.IsAvailable)
        {
            ModelState.AddModelError("BookId", "The selected book is not available for borrowing.");
            PopulateDropdowns(borrowing.BookId, borrowing.ReaderId);
            return View(borrowing);
        }

        book.IsAvailable = false;
        _bookRepo.Update(book);

        borrowing.IsReturned = false;
        _borrowingRepo.Add(borrowing);

        TempData["Success"] = "Borrowing record created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Borrowing/Edit/5
    public IActionResult Edit(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var borrowing = _borrowingRepo.GetById(id);
        if (borrowing is null) return NotFound();

        // When editing, include the currently borrowed book in the dropdown too
        var books = _bookRepo.GetAll()
            .Where(b => b.IsAvailable || b.Id == borrowing.BookId)
            .ToList();
        ViewBag.BookId   = new SelectList(books, "Id", "Title", borrowing.BookId);
        ViewBag.ReaderId = new SelectList(_readerRepo.GetAll(), "Id", "Name", borrowing.ReaderId);

        return View(borrowing);
    }

    // POST: Borrowing/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Borrowing borrowing)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        if (id != borrowing.Id) return BadRequest();

        ModelState.Remove(nameof(Borrowing.Book));
        ModelState.Remove(nameof(Borrowing.Reader));

        if (!ModelState.IsValid)
        {
            var books = _bookRepo.GetAll()
                .Where(b => b.IsAvailable || b.Id == borrowing.BookId)
                .ToList();
            ViewBag.BookId   = new SelectList(books, "Id", "Title", borrowing.BookId);
            ViewBag.ReaderId = new SelectList(_readerRepo.GetAll(), "Id", "Name", borrowing.ReaderId);
            return View(borrowing);
        }

        var existing = _borrowingRepo.GetById(id);
        if (existing is null) return NotFound();

        // Handle book availability changes
        if (existing.BookId != borrowing.BookId)
        {
            var oldBook = _bookRepo.GetById(existing.BookId);
            if (oldBook is not null) { oldBook.IsAvailable = true; _bookRepo.Update(oldBook); }

            var newBook = _bookRepo.GetById(borrowing.BookId);
            if (newBook is not null && newBook.IsAvailable) { newBook.IsAvailable = false; _bookRepo.Update(newBook); }
        }

        // If marked as returned, free up the book
        if (borrowing.IsReturned && !existing.IsReturned)
        {
            var book = _bookRepo.GetById(borrowing.BookId);
            if (book is not null) { book.IsAvailable = true; _bookRepo.Update(book); }

            if (borrowing.ReturnDate is null) borrowing.ReturnDate = DateTime.Today;
        }

        _borrowingRepo.Update(borrowing);
        TempData["Success"] = "Borrowing record updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Borrowing/Delete/5
    public IActionResult Delete(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var borrowing = _borrowingRepo.GetById(id);
        if (borrowing is null) return NotFound();
        return View(Populate(borrowing));
    }

    // POST: Borrowing/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var borrowing = _borrowingRepo.GetById(id);
        if (borrowing is null) return NotFound();

        // Restore book availability if not yet returned
        if (!borrowing.IsReturned)
        {
            var book = _bookRepo.GetById(borrowing.BookId);
            if (book is not null) { book.IsAvailable = true; _bookRepo.Update(book); }
        }

        _borrowingRepo.Delete(id);
        TempData["Success"] = "Borrowing record deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
