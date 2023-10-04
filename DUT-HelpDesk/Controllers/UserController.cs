using DUT_HelpDesk.DatabaseModels;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.WebPages;


namespace DUT_HelpDesk.Controllers
{
    public class UserController : Controller
    {
        private DatabaseModels.DutHelpdeskdbContext db = new DatabaseModels.DutHelpdeskdbContext();
        private readonly ILogger<UserController> _logger;
        FirebaseAuthProvider auth;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            auth = new FirebaseAuthProvider(
                          new FirebaseConfig("AIzaSyDbriiQXcud__j4B6rbGh3brehz9DnBrRM"));
        }

        public IActionResult FaqDashboard()
        {
            List<Faq>faqs = StateManager.GetAllFaqs();
            return View(faqs);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult UserTicket(string? sortBy, string? startDate, string? endDate)
        {
            ViewBag.user = StateManager.user;
            IEnumerable<Ticket> tickets = StateManager.GetUserTickets();
            if(sortBy != null)
            {
                if (sortBy == "Date")
                {
                    tickets = tickets.OrderBy(t => t.DateCreated);
                }
                else if (sortBy == "Status")
                {
                    tickets = tickets.OrderByDescending(t => t.DateCreated);
                }
                else if (sortBy == "Alpha")
                {
                    tickets = tickets.OrderBy(t => t.Subject.ToLower());
                }
            }
            ViewBag.startDate = startDate; ViewBag.endDate = endDate;
            if (startDate != null && endDate != null)
            {
                tickets = tickets.Where(x => x.DateCreated >= DateTime.Parse(startDate) && x.DateCreated <= DateTime.Parse(endDate).AddDays(1)).ToList();
            }
            return View(tickets);
        }
        public IActionResult ViewTicket(int id)
        {
            Ticket ticket = StateManager.GetTicket(id);

            if (ticket == null)
            {
                return StatusCode(400);
            }
            else
            {
                return View(ticket);
            }
        }

        public IActionResult CreateTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                await StateManager.CreateTicket(model, auth);
            }
            IEnumerable<Ticket> tickets = StateManager.GetUserTickets();
            return View("UserTicket", tickets);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new DatabaseModels.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}