using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VehicleRental.Models;
using VehicleRental.Repositories;

namespace VehicleRental.Controllers;

public class HomeController : Controller
{
    private readonly IVehicleRepository     _vehicleRepo;
    private readonly ICustomerRepository    _customerRepo;
    private readonly IReservationRepository _reservationRepo;
    private readonly IBillRepository        _billRepo;

    public HomeController(
        IVehicleRepository vehicleRepo,
        ICustomerRepository customerRepo,
        IReservationRepository reservationRepo,
        IBillRepository billRepo)
    {
        _vehicleRepo     = vehicleRepo;
        _customerRepo    = customerRepo;
        _reservationRepo = reservationRepo;
        _billRepo        = billRepo;
    }

    // GET: / (Dashboard)
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");

        var reservations = _reservationRepo.GetAll().ToList();
        var bills        = _billRepo.GetAll().ToList();

        ViewBag.TotalVehicles       = _vehicleRepo.GetAll().Count();
        ViewBag.AvailableVehicles   = _vehicleRepo.GetAll().Count(v => v.IsAvailable);
        ViewBag.TotalCustomers      = _customerRepo.GetAll().Count();
        ViewBag.ActiveReservations  = reservations.Count(r => r.Status == ReservationStatus.Active);
        ViewBag.PendingReservations = reservations.Count(r => r.Status == ReservationStatus.Pending);
        ViewBag.TotalRevenue        = bills.Where(b => b.IsPaid).Sum(b => b.TotalAmount);
        ViewBag.OutstandingBalance  = bills.Where(b => !b.IsPaid).Sum(b => b.TotalAmount);

        // Recent reservations (last 5) — nav props already loaded by EF Include
        var recent = reservations
            .OrderByDescending(r => r.StartDate)
            .Take(5)
            .ToList();

        return View(recent);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
