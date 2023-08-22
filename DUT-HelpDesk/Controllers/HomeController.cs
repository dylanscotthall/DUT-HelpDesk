using DUT_HelpDesk.Model;
using DUT_HelpDesk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DUT_HelpDesk.Controllers
{
    public class HomeController : Controller
    {
        private DutHelpDeskContext db = new DutHelpDeskContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UserTicket()
        {
            return View(db.Tickets);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}