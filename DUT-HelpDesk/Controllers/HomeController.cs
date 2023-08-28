using DUT_HelpDesk.Model;
using DUT_HelpDesk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace DUT_HelpDesk.Controllers
{
    public class HomeController : Controller
    {
        private DutHelpdeskdbContext db = new DutHelpdeskdbContext();
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
        public IActionResult ViewTicket(int? id)
        {
            if (id == null)
            {
                return StatusCode(400);
            }
            Models.Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return StatusCode(400);
            }
            return View(ticket);
        }

        public IActionResult CreateTicket()
        {
            return View();
        }

        public async Task<IActionResult> CreateTicket(Models.Ticket ticket)
        {
            if(ticket != null)
            {
                await db.Tickets.AddAsync(ticket);
                return RedirectToAction("UserTicket");
            }
            else
            {
                return View(ticket);
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}