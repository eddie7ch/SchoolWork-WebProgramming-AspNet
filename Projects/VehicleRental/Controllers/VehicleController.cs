using Microsoft.AspNetCore.Mvc;
using VehicleRental.Models;
using VehicleRental.Repositories;

namespace VehicleRental.Controllers;

public class VehicleController : Controller
{
    private readonly IVehicleRepository _vehicleRepo;

    public VehicleController(IVehicleRepository vehicleRepo)
    {
        _vehicleRepo = vehicleRepo;
    }

    private IActionResult RequireLogin()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");
        return null!;
    }

    // GET: /Vehicle
    public IActionResult Index()
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;
        return View(_vehicleRepo.GetAll());
    }

    // GET: /Vehicle/Details/5
    public IActionResult Details(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var vehicle = _vehicleRepo.GetById(id);
        return vehicle is null ? NotFound() : View(vehicle);
    }

    // GET: /Vehicle/Create
    public IActionResult Create()
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;
        return View();
    }

    // POST: /Vehicle/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Vehicle vehicle)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        if (!ModelState.IsValid) return View(vehicle);

        _vehicleRepo.Add(vehicle);
        TempData["Success"] = $"{vehicle.DisplayName} has been added.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Vehicle/Edit/5
    public IActionResult Edit(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var vehicle = _vehicleRepo.GetById(id);
        return vehicle is null ? NotFound() : View(vehicle);
    }

    // POST: /Vehicle/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Vehicle vehicle)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        if (id != vehicle.Id) return BadRequest();
        if (!ModelState.IsValid) return View(vehicle);

        _vehicleRepo.Update(vehicle);
        TempData["Success"] = $"{vehicle.DisplayName} has been updated.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Vehicle/Delete/5
    public IActionResult Delete(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var vehicle = _vehicleRepo.GetById(id);
        return vehicle is null ? NotFound() : View(vehicle);
    }

    // POST: /Vehicle/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        _vehicleRepo.Delete(id);
        TempData["Success"] = "Vehicle has been removed.";
        return RedirectToAction(nameof(Index));
    }
}
