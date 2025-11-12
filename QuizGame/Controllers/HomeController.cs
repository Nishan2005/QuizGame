using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizGame.DbContext;
using QuizGame.Models;
using System.Diagnostics;

namespace QuizGame.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, IUserService userService, ApplicationDbContext context)
        {
            _logger = logger;
            _userService = userService;
            _context = context;
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
        [HttpGet]
        public IActionResult GetScore()
        {
            var user = _userService.GetCurrentUser();
            var getScore = _context.Scores.FirstOrDefault(s => s.UserId.ToString() == user.UserId);
            if (getScore != null)
            {
                return Ok(new { YourScore = getScore.TotalScore });
            }
            return Ok(new { YourScore = 0 });
        }
        
        [HttpGet]
        public IActionResult PlusScore()
        {
            try
            {
                var user = _userService.GetCurrentUser();

                var getScore = _context.Scores.FirstOrDefault(s => s.UserId.ToString() == user.UserId);
                if (getScore != null)
                {
                    getScore.TotalScore++;
                    _context.SaveChanges();
                }
                else
                {
                    var createNew = _context.Scores.Add(new Score
                    {
                        UserId = int.Parse(user.UserId),
                        TotalScore = 1
                    });
                    _context.SaveChanges();

                }
                return Ok(true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating score");
                return BadRequest(false);

            }

        }

        public class AnswerModel
        {
            public string Answer { get; set; }
        }

    }
}
