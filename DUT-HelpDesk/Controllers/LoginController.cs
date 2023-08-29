using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DUT_HelpDesk.Controllers
{
    public class LoginController : Controller
    {

        DutHelpdeskdbContext db = new DutHelpdeskdbContext();
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(User user) 
        {
            var checkLogin = db.Users.Where(x => x.UserId.Equals(user.UserId)).FirstOrDefault();

            if (checkLogin == null)
            {
                ViewBag.Error = "Username or Password is Invalid!";
                return View();
            }

            else 
            {
                return RedirectToAction("Index", "Home");   
            }
        }
    }
}
