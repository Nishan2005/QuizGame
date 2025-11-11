using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizGame.Models;
using System.Diagnostics;

namespace QuizGame.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var currentUser = _userService.GetCurrentUser();

            ViewData["Username"] = currentUser.FullName;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public bool SubmitAnswer()
        {
            
            return false;
        }
        [HttpPost]
        public IActionResult CheckAnswer([FromBody] AnswerModel model)
        {
            string correctAnswer = "dog"; // You can change this or fetch dynamically

            bool isCorrect = model.Answer?.Trim().ToLower() == correctAnswer.ToLower();

            return Json(new { isCorrect });
        }

        public class AnswerModel
        {
            public string Answer { get; set; }
        }

    }
}
