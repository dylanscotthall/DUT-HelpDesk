using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Firebase.Auth;
using System.Text.Json;
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
                    if (StateManager.isTechnician())
                    {
                        StateManager.technician = StateManager.GetTechnician();
                    }
                    HttpContext.Session.SetString("_UserType", StateManager.user.Type);

                    if (StateManager.user.Type == "Technician")
                    {
                        return RedirectToAction("TechnicianTicketQueue", "Home");
                    }
                    else
                    {
                        return RedirectToAction("UserTicket", "Home");
                    }
                }
            }
            catch(FirebaseAuthException ex)
            {
                var firebaseEx = JsonSerializer.Deserialize<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                ViewBag.errors = "Login failed. Please check that your credentials are correct.";
                return View(model);
            }
            return View();  
        }



       
    }
}
