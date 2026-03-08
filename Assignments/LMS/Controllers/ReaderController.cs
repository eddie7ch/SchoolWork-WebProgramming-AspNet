using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers;

public class ReaderController : Controller
{
    private readonly IReaderRepository _readerRepo;

    public ReaderController(IReaderRepository readerRepo)
    {
        _readerRepo = readerRepo;
    }

    // GET: Reader
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        return View(_readerRepo.GetAll());
    }

    // GET: Reader/Details/5
    public IActionResult Details(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var reader = _readerRepo.GetById(id);
        if (reader is null) return NotFound();
        return View(reader);
    }

    // GET: Reader/Create
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        return View();
    }

    // POST: Reader/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Reader reader)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid) return View(reader);

        _readerRepo.Add(reader);
        TempData["Success"] = $"Reader \"{reader.Name}\" added successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Reader/Edit/5
    public IActionResult Edit(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var reader = _readerRepo.GetById(id);
        if (reader is null) return NotFound();
        return View(reader);
    }

    // POST: Reader/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Reader reader)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        if (id != reader.Id) return BadRequest();
        if (!ModelState.IsValid) return View(reader);

        _readerRepo.Update(reader);
        TempData["Success"] = $"Reader \"{reader.Name}\" updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Reader/Delete/5
    public IActionResult Delete(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var reader = _readerRepo.GetById(id);
        if (reader is null) return NotFound();
        return View(reader);
    }

    // POST: Reader/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var reader = _readerRepo.GetById(id);
        if (reader is null) return NotFound();

        _readerRepo.Delete(id);
        TempData["Success"] = $"Reader \"{reader.Name}\" deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
