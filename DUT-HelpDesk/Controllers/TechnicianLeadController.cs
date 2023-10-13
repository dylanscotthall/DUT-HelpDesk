using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DUT_HelpDesk.Controllers
{
    public class TechnicianLeadController : Controller
    {

        public IActionResult TechniciansListDashboard(string? sortBy, string? startDate, string? endDate)
        {
            ViewBag.user = StateManager.user;
            ViewBag.technician = StateManager.technician;
            ViewBag.UsersList = StateManager.GetUsers();
            IEnumerable<Technician> technicians = StateManager.GetAllTechnicians();
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
