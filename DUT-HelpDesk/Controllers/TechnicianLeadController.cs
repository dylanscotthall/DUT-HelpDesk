using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages;

namespace DUT_HelpDesk.Controllers
{
    public class TechnicianLeadController : Controller
    {

        public IActionResult TechniciansListDashboard(string? sortBy, string? startDate, string? endDate)
        {
            string userType = StateManager.getUserType();
            if (userType.Equals("TechnicianLead"))
            {
                ViewBag.user = StateManager.user;
                ViewBag.technician = StateManager.technician;
                ViewBag.UsersList = StateManager.GetUsers();
                IEnumerable<Technician> technicians = StateManager.GetAllTechnicians();
                return View(technicians.ToList());
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult ToolsDashboard()
        {
            return View();
        }

        public IActionResult TechnicianInformationDashboard(int techId, string? startDate, string? endDate)
        {
            string userType = StateManager.getUserType();
            if (userType.Equals("TechnicianLead"))
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
            else
            {
                return NotFound();
            }
        }

    }
}
