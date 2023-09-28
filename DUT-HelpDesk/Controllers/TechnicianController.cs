using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DUT_HelpDesk.Controllers
{
    public class TechnicianController : Controller
    {
        public IActionResult TechnicianDashboard()
        {
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            IEnumerable<Ticket> tickets = StateManager.GetTechnicianTickets();
            return View(tickets);
        }
        public IActionResult TechnicianDashboardDetail(int id)
        {
            Ticket ticket = StateManager.GetTicket(id);
            List<Reply> replies = StateManager.GetTicketReplies(id);
            ViewBag.replies = replies;

            if (ticket == null)
            {
                return StatusCode(400);
            }
            else
            {
                return View(ticket);
            }
        }

        public IActionResult TechnicianTicketQueue(string? sortBy, string? startDate, string? endDate, string? status)
        {
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            IEnumerable<Ticket> tickets = StateManager.GetAllTickets();
            if (sortBy != null)
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
            if (status != null)
            {
                tickets = tickets.Where(x => x.TicketStatuses.OrderByDescending(s => s.TimeStamp).FirstOrDefault().Status.Name == status);
            }
            return View(tickets.ToList());
        }
        public IActionResult AssignTicketToTechnician(int? id, int? techId)
        {
            StateManager.CreateTicketTechnician((int)id, (int)techId);
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            return View("TechnicianDashboard", StateManager.GetTechnicianTickets());
        }

        public IActionResult UnassignTicketToTechnician(int? id, int? techId)
        {
            StateManager.UnassignTicketTechnician((int)id, (int)techId);
            ViewBag.technician = StateManager.technician;
            List<Ticket> tickets = StateManager.GetAllTickets();

            return View("TechnicianTicketQueue", tickets);
        }
    }
}
