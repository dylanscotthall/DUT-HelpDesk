using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

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
                else if (sortBy == "Date Down")
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
            //Saved to StateManager for report generation in TechnicianTicketQueueReport
            StateManager.filteredTickets = tickets.ToList(); 
            return View(StateManager.filteredTickets);
        }
        public IActionResult AssignTicketToTechnician(int? id, int? techId)
        {
            StateManager.CreateTicketTechnician((int)id, (int)techId);
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            return View("TechnicianTicketQueue", StateManager.GetAllTickets());
        }

        //When a ticket is unassigned from the technician's assigned tickets page, the technician is returned an updated view of the assigned tickets page.
        public IActionResult UnassignTicketToTechnician(int? id, int? techId)
        {
            StateManager.UnassignTicketTechnician((int)id, (int)techId);
            ViewBag.technician = StateManager.technician;
            return View("TechnicianDashboard", StateManager.GetTechnicianTickets());
        }

        //When a ticket is unassigned from the technician ticket queue page, the technician is returned an updated view of the ticket queue.
        public IActionResult UnassignTicketToTechnicianFromTicketQueue(int? id, int? techId)
        {
            StateManager.UnassignTicketTechnician((int)id, (int)techId);
            ViewBag.technician = StateManager.technician;
            return View("TechnicianTicketQueue", StateManager.GetAllTickets());
        }

        public IActionResult TechnicianTicketQueueReport()
        {
            ViewBag.technician = StateManager.technician;
            var pdf = new ViewAsPdf(StateManager.filteredTickets);
            return pdf;
        }

        public IActionResult MyReplies(int id)
        {
            ReplyTicketViewModel model = new ReplyTicketViewModel();
            Ticket ticket = StateManager.GetTicket(id);
            model.ticket = ticket;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MyReplies(ReplyTicketViewModel vm)
        {
            
                await StateManager.MyReplies(vm);
            
            
            return RedirectToAction("TechnicianDashboard");
        } 
    }
}
