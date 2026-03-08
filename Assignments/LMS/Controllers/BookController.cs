using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers;

public class BookController : Controller
{
    private readonly IBookRepository _bookRepo;

    public BookController(IBookRepository bookRepo)
    {
        _bookRepo = bookRepo;
    }

    // GET: Book
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        return View(_bookRepo.GetAll());
    }

    // GET: Book/Details/5
    public IActionResult Details(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var book = _bookRepo.GetById(id);
        if (book is null) return NotFound();
        return View(book);
    }

    // GET: Book/Create
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        return View();
    }

    // POST: Book/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Book book)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid) return View(book);

        _bookRepo.Add(book);
        TempData["Success"] = $"Book \"{book.Title}\" added successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Book/Edit/5
    public IActionResult Edit(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var book = _bookRepo.GetById(id);
        if (book is null) return NotFound();
        return View(book);
    }

    // POST: Book/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Book book)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        if (id != book.Id) return BadRequest();
        if (!ModelState.IsValid) return View(book);

        _bookRepo.Update(book);
        TempData["Success"] = $"Book \"{book.Title}\" updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Book/Delete/5
    public IActionResult Delete(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var book = _bookRepo.GetById(id);
        if (book is null) return NotFound();
        return View(book);
    }

    // POST: Book/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var book = _bookRepo.GetById(id);
        if (book is null) return NotFound();

        _bookRepo.Delete(id);
        TempData["Success"] = $"Book \"{book.Title}\" deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
