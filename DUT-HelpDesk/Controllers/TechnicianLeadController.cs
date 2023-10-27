using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages;

namespace DUT_HelpDesk.Controllers
{
    public class TechnicianLeadController : Controller
    {

        public IActionResult TechniciansListDashboard()
        {
            if (!StateManager.getUserType()!.Equals("TechnicianLead")) { return NotFound(); }
            {
                ViewBag.user = StateManager.user;
                ViewBag.technician = StateManager.technician;
                ViewBag.UsersList = StateManager.GetUsers();
                List<Technician> technicians = StateManager.GetAllTechnicians();
                return View(technicians.ToList());
            }
        }

        public IActionResult ToolsDashboard()
        {
            return View();
        }

        public IActionResult TechnicianInformationDashboard(int techId, string? startDate, string? endDate)
        {
            if (!StateManager.getUserType()!.Equals("TechnicianLead")) { return NotFound(); }
            {
                Technician technician = StateManager.GetTech(techId);
                List<User> users = StateManager.GetUsers();
                if (technician != null && users.Count > 0)
                {
                    List<Ticket> closedTickets = StateManager.GetTechClosedTickets(technician.TechnicianId);
                    ViewBag.startDate = startDate; ViewBag.endDate = endDate;
                    if (startDate != null && endDate != null)
                    {
                        closedTickets = closedTickets.Where(x => x.DateClosed >= DateTime.Parse(startDate) && x.DateClosed <= DateTime.Parse(endDate).AddDays(1)).ToList();
                    }
                    else
                    {
                        if (startDate != null)
                        {
                            closedTickets = closedTickets.Where(x => x.DateClosed >= DateTime.Parse(startDate)).ToList();
                        }
                        else
                        {
                            if (endDate != null)
                            {
                                closedTickets = closedTickets.Where(x => x.DateClosed <= DateTime.Parse(endDate).AddDays(1)).ToList();
                            }
                        }
                    }
                    ViewBag.AvgFeedbackRating = StateManager.GetAverageFeedbackRating(closedTickets);
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
}
