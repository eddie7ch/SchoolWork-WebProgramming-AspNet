using Microsoft.AspNetCore.Mvc;
using VehicleRental.Models;
using VehicleRental.Repositories;

namespace VehicleRental.Controllers;

public class BillingController : Controller
{
    private readonly IBillRepository        _billRepo;
    private readonly IReservationRepository _reservationRepo;
    private readonly IVehicleRepository     _vehicleRepo;
    private readonly ICustomerRepository    _customerRepo;

    public BillingController(
        IBillRepository billRepo,
        IReservationRepository reservationRepo,
        IVehicleRepository vehicleRepo,
        ICustomerRepository customerRepo)
    {
        _billRepo        = billRepo;
        _reservationRepo = reservationRepo;
        _vehicleRepo     = vehicleRepo;
        _customerRepo    = customerRepo;
    }

    private IActionResult RequireLogin()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");
        return null!;
    }

    private void PopulateReservationNavProps(Bill bill)
    {
        if (bill.Reservation is null) return;
        bill.Reservation.Customer = _customerRepo.GetById(bill.Reservation.CustomerId);
        bill.Reservation.Vehicle  = _vehicleRepo.GetById(bill.Reservation.VehicleId);
    }

    // GET: /Billing
    public IActionResult Index()
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var bills = _billRepo.GetAll()
            .Select(b =>
            {
                b.Reservation = _reservationRepo.GetById(b.ReservationId);
                if (b.Reservation is not null)
                {
                    b.Reservation.Customer = _customerRepo.GetById(b.Reservation.CustomerId);
                    b.Reservation.Vehicle  = _vehicleRepo.GetById(b.Reservation.VehicleId);
                }
                return b;
            })
            .ToList();

        return View(bills);
    }

    // GET: /Billing/Details/5
    public IActionResult Details(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var bill = _billRepo.GetById(id);
        if (bill is null) return NotFound();

        bill.Reservation = _reservationRepo.GetById(bill.ReservationId);
        PopulateReservationNavProps(bill);

        return View(bill);
    }

    // POST: /Billing/MarkPaid/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult MarkPaid(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var bill = _billRepo.GetById(id);
        if (bill is null) return NotFound();

        bill.IsPaid = true;
        _billRepo.Update(bill);

        // Mark reservation as completed
        var reservation = _reservationRepo.GetById(bill.ReservationId);
        if (reservation is not null)
        {
            reservation.Status = ReservationStatus.Completed;
            _reservationRepo.Update(reservation);

            // Free the vehicle
            var vehicle = _vehicleRepo.GetById(reservation.VehicleId);
            if (vehicle is not null)
            {
                vehicle.IsAvailable = true;
                _vehicleRepo.Update(vehicle);
            }
        }

        TempData["Success"] = "Payment recorded. Reservation marked as completed.";
        return RedirectToAction(nameof(Index));
    }
}
