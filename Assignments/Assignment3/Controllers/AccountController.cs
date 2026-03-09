using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _users;

    public AccountController(IUserRepository users) => _users = users;

    // GET: Account/Login
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("Username") is not null)
            return RedirectToAction("Index", "Home");
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = _users.GetByUsername(model.Username);
        if (user is null || !UserRepository.VerifyPassword(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        return RedirectToAction("Index", "Home");
    }

    // GET: Account/Register
    public IActionResult Register()
    {
        if (HttpContext.Session.GetString("Username") is not null)
            return RedirectToAction("Index", "Home");
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (_users.UsernameExists(model.Username))
        {
            ModelState.AddModelError("Username", "This username is already taken.");
            return View(model);
        }

        if (_users.GetByEmail(model.Email) is not null)
        {
            ModelState.AddModelError("Email", "An account with this email already exists.");
            return View(model);
        }

        _users.Add(new User
        {
            Username     = model.Username,
            Email        = model.Email,
            PasswordHash = UserRepository.HashPassword(model.Password)
        });

        TempData["Success"] = "Account created! Please log in.";
        return RedirectToAction(nameof(Login));
    }

    // POST: Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
