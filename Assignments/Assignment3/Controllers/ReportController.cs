using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers;

public class ReportController : Controller
{
    private readonly IBookRepository _books;
    private readonly IReaderRepository _readers;
    private readonly IBorrowingRepository _borrowings;

    public ReportController(IBookRepository books, IReaderRepository readers, IBorrowingRepository borrowings)
    {
        _books = books;
        _readers = readers;
        _borrowings = borrowings;
    }

    private bool IsLoggedIn => HttpContext.Session.GetString("Username") is not null;

    // GET: Report/Index
    public IActionResult Index()
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");

        var books = _books.GetAll().ToList();
        var readers = _readers.GetAll().ToList();
        var borrowings = _borrowings.GetAll().ToList();

        ViewBag.TotalBooks = books.Count;
        ViewBag.TotalCopies = books.Sum(b => b.TotalCopies);
        ViewBag.AvailableCopies = books.Sum(b => b.AvailableCopies);
        ViewBag.TotalReaders = readers.Count;
        ViewBag.TotalBorrowings = borrowings.Count;
        ViewBag.ActiveBorrowings = borrowings.Count(b => !b.IsReturned);
        ViewBag.ReturnedBorrowings = borrowings.Count(b => b.IsReturned);
        ViewBag.OverdueBorrowings = borrowings.Count(b => b.IsOverdue);
        ViewBag.TotalFinesCollected = borrowings.Where(b => b.IsReturned).Sum(b => b.OverdueFee);
        ViewBag.OutstandingFines = borrowings.Where(b => !b.IsReturned).Sum(b => b.OverdueFee);

        // Most borrowed books
        var bookBorrowCounts = borrowings
            .GroupBy(b => b.BookId)
            .Select(g => new { BookId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        var topBooks = bookBorrowCounts
            .Select(x => new { Book = _books.GetById(x.BookId), x.Count })
            .Where(x => x.Book is not null)
            .ToList();

        ViewBag.TopBooks = topBooks;

        return View();
    }

    // GET: Report/Overdue
    public IActionResult Overdue()
    {
        if (!IsLoggedIn) return RedirectToAction("Login", "Account");

        var borrowings = _borrowings.GetAll().Where(b => b.IsOverdue).ToList();
        foreach (var b in borrowings)
        {
            b.Book ??= _books.GetById(b.BookId);
            b.Reader ??= _readers.GetById(b.ReaderId);
        }

        return View(borrowings.OrderByDescending(b => b.OverdueDays).ToList());
    }
}
