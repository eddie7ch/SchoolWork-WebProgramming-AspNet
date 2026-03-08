using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRental.Models;
using VehicleRental.Repositories;

namespace VehicleRental.Controllers;

public class ReservationController : Controller
{
    private readonly IReservationRepository _reservationRepo;
    private readonly IVehicleRepository     _vehicleRepo;
    private readonly ICustomerRepository    _customerRepo;
    private readonly IBillRepository        _billRepo;

    public ReservationController(
        IReservationRepository reservationRepo,
        IVehicleRepository vehicleRepo,
        ICustomerRepository customerRepo,
        IBillRepository billRepo)
    {
        _reservationRepo = reservationRepo;
        _vehicleRepo     = vehicleRepo;
        _customerRepo    = customerRepo;
        _billRepo        = billRepo;
    }

    private IActionResult RequireLogin()
    {
        if (HttpContext.Session.GetString("Username") is null)
            return RedirectToAction("Login", "Account");
        return null!;
    }

    private void PopulateDropdowns()
    {
        ViewBag.Customers = _customerRepo.GetAll()
            .Select(c => new SelectListItem(c.FullName, c.Id.ToString()));
        ViewBag.Vehicles = _vehicleRepo.GetAll()
            .Where(v => v.IsAvailable)
            .Select(v => new SelectListItem(v.DisplayName, v.Id.ToString()));
    }

    // GET: /Reservation
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
            .OrderByDescending(r => r.StartDate)
            .ToList();

        return View(reservations);
    }

    // GET: /Reservation/Details/5
    public IActionResult Details(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var reservation = _reservationRepo.GetById(id);
        if (reservation is null) return NotFound();

        reservation.Customer = _customerRepo.GetById(reservation.CustomerId);
        reservation.Vehicle  = _vehicleRepo.GetById(reservation.VehicleId);
        ViewBag.Bill = _billRepo.GetByReservationId(id);

        return View(reservation);
    }

    // GET: /Reservation/Create
    public IActionResult Create()
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;
        PopulateDropdowns();
        return View(new Reservation { StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1) });
    }

    // POST: /Reservation/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Reservation reservation)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        if (reservation.EndDate <= reservation.StartDate)
            ModelState.AddModelError("EndDate", "End date must be after start date.");

        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(reservation);
        }

        _reservationRepo.Add(reservation);

        // Mark vehicle as unavailable
        var vehicle = _vehicleRepo.GetById(reservation.VehicleId);
        if (vehicle is not null)
        {
            vehicle.IsAvailable = false;
            _vehicleRepo.Update(vehicle);
        }

        // Auto-generate bill
        if (vehicle is not null)
        {
            var bill = new Bill
            {
                ReservationId = reservation.Id,
                BaseAmount    = vehicle.DailyRate * reservation.RentalDays,
                TaxRate       = 10m,
            };
            _billRepo.Add(bill);
        }

        TempData["Success"] = "Reservation created.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Reservation/Edit/5
    public IActionResult Edit(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var reservation = _reservationRepo.GetById(id);
        if (reservation is null) return NotFound();

        // Allow all vehicles in dropdown when editing (current vehicle may be unavailable)
        ViewBag.Customers = _customerRepo.GetAll()
            .Select(c => new SelectListItem(c.FullName, c.Id.ToString()));
        ViewBag.Vehicles = _vehicleRepo.GetAll()
            .Select(v => new SelectListItem(v.DisplayName, v.Id.ToString()));

        return View(reservation);
    }

    // POST: /Reservation/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Reservation reservation)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        if (id != reservation.Id) return BadRequest();

        if (reservation.EndDate <= reservation.StartDate)
            ModelState.AddModelError("EndDate", "End date must be after start date.");

        if (!ModelState.IsValid)
        {
            ViewBag.Customers = _customerRepo.GetAll()
                .Select(c => new SelectListItem(c.FullName, c.Id.ToString()));
            ViewBag.Vehicles = _vehicleRepo.GetAll()
                .Select(v => new SelectListItem(v.DisplayName, v.Id.ToString()));
            return View(reservation);
        }

        _reservationRepo.Update(reservation);
        TempData["Success"] = "Reservation updated.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Reservation/Cancel/5
    public IActionResult Cancel(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var reservation = _reservationRepo.GetById(id);
        if (reservation is null) return NotFound();

        reservation.Customer = _customerRepo.GetById(reservation.CustomerId);
        reservation.Vehicle  = _vehicleRepo.GetById(reservation.VehicleId);
        return View(reservation);
    }

    // POST: /Reservation/Cancel/5
    [HttpPost, ActionName("Cancel")]
    [ValidateAntiForgeryToken]
    public IActionResult CancelConfirmed(int id)
    {
        var redirect = RequireLogin();
        if (redirect is not null) return redirect;

        var reservation = _reservationRepo.GetById(id);
        if (reservation is null) return NotFound();

        reservation.Status = ReservationStatus.Cancelled;
        _reservationRepo.Update(reservation);

        // Free up the vehicle
        var vehicle = _vehicleRepo.GetById(reservation.VehicleId);
        if (vehicle is not null)
        {
            vehicle.IsAvailable = true;
            _vehicleRepo.Update(vehicle);
        }

        TempData["Success"] = "Reservation has been cancelled.";
        return RedirectToAction(nameof(Index));
    }
}
