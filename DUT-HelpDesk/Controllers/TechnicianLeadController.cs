using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DUT_HelpDesk.Controllers
{
    public class TechnicianLeadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TechnicianLeadDashboard()
        {
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            IEnumerable<Ticket> tickets = StateManager.GetAllTickets();
            return View(tickets);
        }

        public IActionResult TechnicianLeadDashboardDetail(int id)
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
    }
}
