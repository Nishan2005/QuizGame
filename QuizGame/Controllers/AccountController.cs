using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizGame.DbContext;
using QuizGame.Models;
using QuizGame.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;

    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || user.Password != password)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View();
        }

        // Create user claims
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
        new Claim(ClaimTypes.Email, user.Email)
    };

        // Create the identity and principal
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(SignupModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        if (_context.Users.Any(u => u.Email == model.SEmail))
        {
            ModelState.AddModelError("Email", "This email is already registered.");
            return View();
        }


        try
        {
            var user = new User
            {
                FullName = model.FullName,
                Email = model.SEmail,
                Password = model.Password 
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            ViewBag.Message = "User registered successfully!";
            return View("Login");
        }
        catch (Exception ex)
        {
            ViewBag.Message = "Error: " + ex.Message;
            return View(model);
        }
    }




}
