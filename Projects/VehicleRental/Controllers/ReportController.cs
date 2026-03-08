using Microsoft.AspNetCore.Mvc;
using VehicleRental.Models;
using VehicleRental.Repositories;

namespace VehicleRental.Controllers;

public class ReportController : Controller
{
    private readonly IVehicleRepository     _vehicleRepo;
    private readonly ICustomerRepository    _customerRepo;
    private readonly IReservationRepository _reservationRepo;
    private readonly IBillRepository        _billRepo;

    public ReportController(
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

    private IActionResult RequireLogin()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");
        return null!;
    }

    // GET: /Report
    public IActionResult Index()
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var reservations = _reservationRepo.GetAll()
            .Select(r =>
            {
                r.Customer = _customerRepo.GetById(r.CustomerId);
                r.Vehicle  = _vehicleRepo.GetById(r.VehicleId);
                return r;
            })
            .ToList();

        var bills = _billRepo.GetAll()
            .Select(b =>
            {
                b.Reservation = _reservationRepo.GetById(b.ReservationId);
                return b;
            })
            .ToList();

        var vehicles = _vehicleRepo.GetAll().ToList();

        // Revenue by vehicle type
        ViewBag.RevenueByType = bills
            .Where(b => b.IsPaid && b.Reservation is not null)
            .GroupBy(b => _vehicleRepo.GetById(b.Reservation!.VehicleId)?.VehicleType)
            .Select(g => new { Type = g.Key?.ToString() ?? "Unknown", Revenue = g.Sum(b => b.TotalAmount) })
            .ToList();

        // Reservations by status
        ViewBag.ReservationsByStatus = Enum.GetValues<ReservationStatus>()
            .Select(s => new { Status = s.ToString(), Count = reservations.Count(r => r.Status == s) })
            .ToList();

        // Top customers
        ViewBag.TopCustomers = reservations
            .GroupBy(r => r.CustomerId)
            .Select(g => new
            {
                Customer   = _customerRepo.GetById(g.Key)?.FullName ?? "Unknown",
                TotalTrips = g.Count()
            })
            .OrderByDescending(x => x.TotalTrips)
            .Take(5)
            .ToList();

        ViewBag.TotalRevenue   = bills.Where(b => b.IsPaid).Sum(b => b.TotalAmount);
        ViewBag.TotalReservations = reservations.Count;
        ViewBag.FleetUtilization  = vehicles.Count == 0 ? 0 :
            (double)vehicles.Count(v => !v.IsAvailable) / vehicles.Count * 100;

        return View();
    }
}
