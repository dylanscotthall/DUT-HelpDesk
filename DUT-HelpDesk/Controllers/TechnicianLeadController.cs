using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages;
using Firebase.Auth;

namespace DUT_HelpDesk.Controllers
{
    public class TechnicianLeadController : Controller
    {
        FirebaseAuthProvider auth;

        public TechnicianLeadController()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            auth = new FirebaseAuthProvider(
                           new FirebaseConfig(config["FirebaseKey"]));
        }

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
            
                Technician technician = StateManager.GetTech(techId);
                List<DatabaseModels.User> users = StateManager.GetUsers();
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

        public IActionResult RegisterTechnician()
        {
            if (!StateManager.getUserType()!.Equals("TechnicianLead")) { return NotFound(); }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTechnician(RegisterViewModel model)
        {
            if (!StateManager.getUserType()!.Equals("TechnicianLead")) { return NotFound(); }
            using var db = new DutHelpdeskdbContext();
            try
            {
                await auth.CreateUserWithEmailAndPasswordAsync(model.Email, model.Password);

                var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(model.Email, model.Password);
                string token = fbAuthLink.FirebaseToken;
                var fbUser = await auth.GetUserAsync(token);

                DatabaseModels.User user = new DatabaseModels.User
                {
                    Email = model.Email,
                    FbId = fbUser.LocalId,
                    Type = "Technician"
                };
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();

                using var db2 = new DutHelpdeskdbContext();
                DatabaseModels.User newUser = db2.Users.Where(x => x.Email == model.Email).FirstOrDefault()!;

                    Technician tech = new Technician
                    {
                        UserId = newUser.UserId,
                        DateJoined = DateTime.UtcNow
                    };
                    await db2.Technicians.AddAsync(tech);
                    await db2.SaveChangesAsync();
                


                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    StateManager.token = token;
                    HttpContext.Session.SetString("_UserID", fbAuthLink.User.LocalId);
                    StateManager.user = StateManager.GetUserByFBId(fbAuthLink.User.LocalId);
                    HttpContext.Session.SetString("_UserType", StateManager.user.Type);
                    StateManager.email = fbAuthLink.User.Email;

                    return RedirectToAction("TechnicianTicketQueue", "Technician");

                }
            }
            catch (FirebaseAuthException)
            {
                ModelState.AddModelError(string.Empty, "Technician could not be registered.");
                return View(model);
            }
            return View();
        }

    }
}
