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
            auth = new FirebaseAuthProvider(
                           new FirebaseConfig("AIzaSyDbriiQXcud__j4B6rbGh3brehz9DnBrRM"));
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
                    HttpContext.Session.SetString("_UserID", fbAuthLink.User.LocalId);
                    UserMethods.user = UserMethods.GetUserByFBId(fbAuthLink.User.LocalId);
                    if (UserMethods.isTechnician())
                    {
                        UserMethods.technician = UserMethods.GetTechnician();
                    }
                    HttpContext.Session.SetString("_UserType", UserMethods.user.Type);

                    if (UserMethods.user.Type == "Technician")
                    {
                        return RedirectToAction("TechnicianLeadDashboard", "Home");
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
