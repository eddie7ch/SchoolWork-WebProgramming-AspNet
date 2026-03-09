using System.Diagnostics;
using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers;

public class HomeController : Controller
{
    private readonly IBookRepository _books;
    private readonly IReaderRepository _readers;
    private readonly IBorrowingRepository _borrowings;

    public HomeController(IBookRepository books, IReaderRepository readers, IBorrowingRepository borrowings)
    {
        _books = books;
        _readers = readers;
        _borrowings = borrowings;
    }

    // GET: / (Dashboard)
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var books      = _books.GetAll().ToList();
        var readers    = _readers.GetAll().ToList();
        var borrowings = _borrowings.GetAll().ToList();

        ViewBag.TotalBooks        = books.Count;
        ViewBag.AvailableBooks    = books.Sum(b => b.AvailableCopies);
        ViewBag.TotalReaders      = readers.Count;
        ViewBag.ActiveBorrowings  = borrowings.Count(b => !b.IsReturned);
        ViewBag.OverdueBorrowings = borrowings.Count(b => b.IsOverdue);
        ViewBag.OutstandingFees   = borrowings.Where(b => !b.IsReturned).Sum(b => b.OverdueFee);

        var recent = borrowings.OrderByDescending(b => b.BorrowDate).Take(5).ToList();
        foreach (var b in recent)
        {
            b.Book   ??= _books.GetById(b.BookId);
            b.Reader ??= _readers.GetById(b.ReaderId);
        }

        return View(recent);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
