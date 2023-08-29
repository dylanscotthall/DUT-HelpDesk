using DUT_HelpDesk.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using System.Threading.Tasks;


namespace DUT_HelpDesk.Controllers
{
    public class LoginController : Controller
    {

        DutHelpdeskdbContext db = new DutHelpdeskdbContext();

        public async Task<ActionResult> SignIn() 
        {
            var userId = "12345";
            var currentLoginTime = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");

            //Save non identifying data to Firebase
            var currentUserLogin = new LoginData() { TimestampUtc = currentLoginTime };
            var firebaseClient = new FirebaseClient("https://dut-helpdesk-default-rtdb.firebaseio.com/");
            var result = await firebaseClient
              .Child("Users/" + userId + "/Logins")
              .PostAsync(currentUserLogin);

            //Retrieve data from Firebase
            var dbLogins = await firebaseClient
              .Child("Users")
              .Child(userId)
              .Child("Logins")
              .OnceAsync<LoginData>();

            var timestampList = new List<DateTime>();

            //Convert JSON data to original datatype
            foreach (var login in dbLogins)
            {
                timestampList.Add(Convert.ToDateTime(login.Object.TimestampUtc).ToLocalTime());
            }

            //Pass data to the view
            ViewBag.CurrentUser = userId;
            ViewBag.Logins = timestampList.OrderByDescending(x => x);
            return View();
        }
        //public IActionResult SignIn()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult SignIn(User user) 
        //{
        //    var checkLogin = db.Users.Where(x => x.UserId.Equals(user.UserId)).FirstOrDefault();

        //    if (checkLogin == null)
        //    {
        //        ViewBag.Error = "Username or Password is Invalid!";
        //        return View();
        //    }

        //    else 
        //    {
        //        return RedirectToAction("Index", "Home");   
        //    }
        //}
    }
}
