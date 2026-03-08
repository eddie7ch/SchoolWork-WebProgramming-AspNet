using Microsoft.AspNetCore.Mvc;
using VehicleRental.Models;
using VehicleRental.Repositories;

namespace VehicleRental.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerRepository _customerRepo;

    public CustomerController(ICustomerRepository customerRepo)
    {
        _customerRepo = customerRepo;
    }

    private IActionResult RequireLogin()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");
        return null!;
    }

    // GET: /Customer
    public IActionResult Index()
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;
        return View(_customerRepo.GetAll());
    }

    // GET: /Customer/Details/5
    public IActionResult Details(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var customer = _customerRepo.GetById(id);
        return customer is null ? NotFound() : View(customer);
    }

    // GET: /Customer/Create
    public IActionResult Create()
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;
        return View();
    }

    // POST: /Customer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        if (!ModelState.IsValid) return View(customer);

        _customerRepo.Add(customer);
        TempData["Success"] = $"{customer.FullName} has been added.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Customer/Edit/5
    public IActionResult Edit(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var customer = _customerRepo.GetById(id);
        return customer is null ? NotFound() : View(customer);
    }

    // POST: /Customer/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Customer customer)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        if (id != customer.Id) return BadRequest();
        if (!ModelState.IsValid) return View(customer);

        _customerRepo.Update(customer);
        TempData["Success"] = $"{customer.FullName} has been updated.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Customer/Delete/5
    public IActionResult Delete(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var customer = _customerRepo.GetById(id);
        return customer is null ? NotFound() : View(customer);
    }

    // POST: /Customer/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        _customerRepo.Delete(id);
        TempData["Success"] = "Customer has been removed.";
        return RedirectToAction(nameof(Index));
    }
}
