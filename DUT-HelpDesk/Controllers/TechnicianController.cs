using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            ViewBag.userType = StateManager.getUserType();
            Ticket ticket = StateManager.GetTicket(id);
            List<Reply> replies = StateManager.GetTicketReplies(id);
            ViewBag.replies = replies;
            List<User> users = StateManager.GetUsers();
            ViewBag.users = users;
            if (ticket == null)
            {
                return StatusCode(400);
            }
            else
            {
                ReplyTicketViewModel model = new ReplyTicketViewModel();
                model.isClosed = StateManager.TicketIsClosed(id);
                model.ticket = ticket;
                return View(model);
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
            else
            {
                if (startDate != null)
                {
                    tickets = tickets.Where(x => x.DateCreated >= DateTime.Parse(startDate)).ToList();
                }
                else
                {
                    if (endDate != null)
                    {
                        tickets = tickets.Where(x => x.DateCreated <= DateTime.Parse(endDate).AddDays(1)).ToList();
                    }
                }
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
        public IActionResult TechCreateFAQ()
        {
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            IEnumerable<Faq> tickets = StateManager.GetAllFaqs();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TechCreateFAQ(Faq faq)
        {
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            faq.TechnicianId = StateManager.technician.TechnicianId;
            StateManager.CreateFaq(faq);
            return RedirectToAction("FaqDashboard");
        }

        public IActionResult TechnicianTicketQueueReport()
        {
            ViewBag.technician = StateManager.technician;
            var pdf = new ViewAsPdf(StateManager.filteredTickets);
            return pdf;
        }


        [HttpPost]
        public async Task<IActionResult> MyReplies(ReplyTicketViewModel vm)
        {
            if (vm.Message != null)
            {
                await StateManager.MyReplies(vm);
            }
            return RedirectToAction("TechnicianDashboardDetail", new { id = vm.id });
        }
        public async Task<IActionResult> ViewReplyAttachment(int id)
        {

            DatabaseModels.DutHelpdeskdbContext db = new DutHelpdeskdbContext();
            var uploadedFile = await db.Attachments.FirstOrDefaultAsync(f => f.ReplyId == id);

            if (uploadedFile == null)
            {
                return NotFound();
            }

            return File(uploadedFile.FileContent, uploadedFile.ContentType); // Adjust the content type as needed
        }
        public async Task<IActionResult> ViewAttachment(int id)
        {
            DatabaseModels.DutHelpdeskdbContext db = new DutHelpdeskdbContext();

            var uploadedFile = await db.Attachments.FirstOrDefaultAsync(f => f.TicketId == id);

            if (uploadedFile == null)
            {
                return NotFound();
            }

            return File(uploadedFile.FileContent, uploadedFile.ContentType); // Adjust the content type as needed
        }


        public IActionResult FaqDashboard()
        {
            List<Faq> faqs = StateManager.GetAllFaqs();
            return View(faqs);

        }

        public IActionResult TechViewTicket(int id)
        {
            Ticket ticket = StateManager.GetTicket(id);
            List<Reply> replies = StateManager.GetTicketReplies(id);
            ViewBag.replies = replies;
            List<User> users = StateManager.GetUsers();
            ViewBag.users = users;
            if (ticket == null)
            {
                return StatusCode(400);
            }
            else
            {
                ReplyTicketViewModel model = new ReplyTicketViewModel();
                model.ticket = ticket;
                return View(model);
            }
        }

        public async Task<IActionResult> CloseTicket()
        {
            var vticketId = Request.Form["ticketId"];
            int ticketId = Convert.ToInt32(vticketId);
            await StateManager.CloseTicket(ticketId);
            int count = StateManager.GetTechnicianClosedTicketCount(StateManager.technician.TechnicianId);
            return RedirectToAction("TechnicianDashboardDetail", new { id = ticketId });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePriority(ReplyTicketViewModel vm)
        {
            if (vm.Priority.HasValue)
            {
                string p = vm.Priority.Value.ToString();
                await StateManager.ChangeTicketPriority(vm.id, p);
            }
            return RedirectToAction("TechnicianDashboardDetail", new { id = vm.id });
        }


        public IActionResult TechnicianInsightsDashboard(string? startDate, string? endDate)
        {
            Technician technician = StateManager.GetTechnician();
            List<User> users = StateManager.GetUsers();
            if (technician != null && users.Count>0)
            {
                List<Ticket> closedTickets = StateManager.GetTechClosedTickets(technician.TechnicianId);
                ViewBag.startDate = startDate; ViewBag.endDate = endDate;
                if (startDate != null && endDate != null)
                {
                    closedTickets = closedTickets.Where(x => x.DateCreated >= DateTime.Parse(startDate) && x.DateCreated <= DateTime.Parse(endDate).AddDays(1)).ToList();
                }
                else
                {
                    if (startDate != null)
                    {
                        closedTickets = closedTickets.Where(x => x.DateCreated >= DateTime.Parse(startDate)).ToList();
                    }
                    else
                    {
                        if (endDate != null)
                        {
                            closedTickets = closedTickets.Where(x => x.DateCreated <= DateTime.Parse(endDate).AddDays(1)).ToList();
                        }
                    }
                }
                ViewBag.AvgResolutionTime = StateManager.GetAverageResolutionTime(closedTickets);
                ViewBag.ClosedTickets = closedTickets;
                ViewBag.UsersList = users;
                return View(technician);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
