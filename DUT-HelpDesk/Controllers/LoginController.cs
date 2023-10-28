using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Firebase.Auth;
namespace DUT_HelpDesk.Controllers
{
    public class LoginController : Controller
    {
        FirebaseAuthProvider auth;
        DutHelpdeskdbContext db = new DutHelpdeskdbContext();

        public LoginController()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            auth = new FirebaseAuthProvider(
                           new FirebaseConfig(config["FirebaseKey"]));
        }
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var fbAuthLink = await auth
                               .SignInWithEmailAndPasswordAsync(model.Email, model.Password);
                string token = fbAuthLink.FirebaseToken;

                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    StateManager.token = token;
                    HttpContext.Session.SetString("_UserID", fbAuthLink.User.LocalId);
                    StateManager.user = StateManager.GetUserByFBId(fbAuthLink.User.LocalId);
                    HttpContext.Session.SetString("_UserType", StateManager.user.Type);
                    StateManager.email = fbAuthLink.User.Email;
                    
                    switch (StateManager.getUserType())
                    {
                        case "Student":
                            return RedirectToAction("UserTicket", "User");
                        case "Technician":
                            StateManager.technician = StateManager.GetTechnician();
                            return RedirectToAction("TechnicianTicketQueue", "Technician");
                        case "TechnicianLead":
                            StateManager.technician = StateManager.GetTechnician();
                            return RedirectToAction("TechnicianTicketQueue", "Technician");
                    }
                }
            }
            catch(FirebaseAuthException)
            {
                ModelState.AddModelError(string.Empty, "Invalid Credentials");
                ViewBag.errors = "Login failed. Please check that your credentials are correct.";
                return View(model);
            }
            return View();  
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                await auth.CreateUserWithEmailAndPasswordAsync(model.Email,model.Password);
                
                var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(model.Email,model.Password);
                string token = fbAuthLink.FirebaseToken;
                var fbUser = await auth.GetUserAsync(token);

                DatabaseModels.User user = new DatabaseModels.User();
                user.Email = model.Email;
                user.FbId = fbUser.LocalId;
                user.Type = "Student";
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    StateManager.token = token;
                    HttpContext.Session.SetString("_UserID", fbAuthLink.User.LocalId);
                    StateManager.user = StateManager.GetUserByFBId(fbAuthLink.User.LocalId);
                    HttpContext.Session.SetString("_UserType", StateManager.user.Type);
                    StateManager.email = fbAuthLink.User.Email;

                    switch (StateManager.getUserType())
                    {
                        case "Student":
                            return RedirectToAction("UserTicket", "User");
                        case "Technician":
                            StateManager.technician = StateManager.GetTechnician();
                            return RedirectToAction("TechnicianTicketQueue", "Technician");
                        case "TechnicianLead":
                            StateManager.technician = StateManager.GetTechnician();
                            return RedirectToAction("TechnicianTicketQueue", "Technician");
                    }
                }
            }
            catch (FirebaseAuthException)
            {
                ModelState.AddModelError(string.Empty, "Registration Unsuccessful");
                return View(model);
            }
            return View();
        }

        public IActionResult Logout()
        {
            StateManager.user = null;
            StateManager.technician = null;
            StateManager.filteredTickets = null;
            StateManager.email = null;
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
