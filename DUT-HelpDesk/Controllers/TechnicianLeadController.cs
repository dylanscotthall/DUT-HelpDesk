using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DUT_HelpDesk.Controllers
{
    public class TechnicianLeadController : Controller
    {
        public IActionResult TechnicianLeadDashboard()
        {
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            IEnumerable<Ticket> tickets = StateManager.GetTechnicianTickets();
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
        public IActionResult TechnicianLeadTicketQueue(string? sortBy, string? startDate, string? endDate, string? status)
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
            return View("TechnicianLeadTicketQueue", StateManager.GetAllTickets());
        }

        //When a ticket is unassigned from the technician's assigned tickets page, the technician is returned an updated view of the assigned tickets page.
        public IActionResult UnassignTicketToTechnician(int? id, int? techId)
        {
            StateManager.UnassignTicketTechnician((int)id, (int)techId);
            ViewBag.technician = StateManager.technician;
            return View("TechnicianLeadTicketQueue", StateManager.GetAllTickets());
        }
        public IActionResult TechniciansListDashboard(string? sortBy, string? startDate, string? endDate)
        {
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            ViewBag.UsersList = StateManager.GetUsers();
            IEnumerable<Technician> technicians = StateManager.GetAllTechnicians();
            //checking for count of completed tickets, waiting on ticket completion
            //foreach (Technician item in technicians)
            //{
            //    var s = item.TicketTechnicians.Where(x => x.Ticket.TicketStatuses.OrderByDescending(o => o.TimeStamp).FirstOrDefault().Status.StatusId == 3).ToList().Count;
            //    int t = 5;
            //}
            if (sortBy != null)
            {
                if (sortBy == "Date")
                {
                    technicians = technicians.OrderBy(t => t.DateJoined);
                }
                else if (sortBy == "Date Down")
                {
                    technicians = technicians.OrderByDescending(t => t.DateJoined);
                }
                //else if (sortBy == "Alpha")
                //{
                //    technicians = technicians.OrderBy(t => t.User.ToLower());
                //}
            }
            ViewBag.startDate = startDate; ViewBag.endDate = endDate;
            if (startDate != null && endDate != null)
            {
                technicians = technicians.Where(x => x.DateJoined >= DateTime.Parse(startDate) && x.DateJoined <= DateTime.Parse(endDate).AddDays(1)).ToList();
            }
            return View(technicians.ToList());
        }
        public IActionResult InsightsDashboard()
        {
            return View();
        }
        public IActionResult ToolsDashboard()
        {
            return View();
        }
    }
}
