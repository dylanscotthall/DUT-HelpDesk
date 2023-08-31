using DUT_HelpDesk.DatabaseModels;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text;


namespace DUT_HelpDesk.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseModels.DutHelpdeskdbContext db = new DatabaseModels.DutHelpdeskdbContext();
        private readonly ILogger<HomeController> _logger;
        FirebaseAuthProvider auth;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            auth = new FirebaseAuthProvider(
                          new FirebaseConfig("AIzaSyDbriiQXcud__j4B6rbGh3brehz9DnBrRM"));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UserTicket()
        {
            return View(db.Tickets);
        }
        public IActionResult ViewTicket(int? id)
        {
            if (id == null)
            {
                return StatusCode(400);
            }

            Ticket ticket = db.Tickets.Find(id);
            

            if (ticket == null)
            {
                return StatusCode(400);
            }
            else
            {
                return View(ticket);
            }
        }

        public IActionResult CreateTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketViewModel model)
        {
            
            
              
            if (ModelState.IsValid)
            {
                //get current user
                string token =  HttpContext.Session.GetString("_UserToken");
                var userFbId = await auth.GetUserAsync(token);

                var currentUser = db.Users.Where(u => u.FbId.Equals(userFbId.LocalId)).FirstOrDefault();
                Ticket ticket = new Ticket()
                {
                    //userid 
                    UserId = currentUser.UserId,
                    //technicianid to be assigned later
                    Subject = model.Subject,
                    QueryBody = model.QueryBody,
                    Status = "Available",
                    Priority = "Low",
                    DateCreated = DateTime.Now,
                    
                };

                await db.Tickets.AddAsync(ticket);
                await db.SaveChangesAsync();

               
                if(model.File != null)
                {
                
                    using (var stream = new MemoryStream())
                    {
                        await model.File.CopyToAsync(stream);

                        stream.Seek(0, SeekOrigin.Begin);

                        Ticket[] currentTicket = db.Tickets.Where(t => t == ticket).ToArray();
                        var uploadedFile = new Attachment()
                        {
                            TicketId = currentTicket[0].TicketId,
                            FileName = model.File.Name,
                            FileContent = stream.ToArray(),
                            ContentType = model.File.ContentType
                        };
                        
                        await db.Attachments.AddAsync(uploadedFile);
                        await db.SaveChangesAsync();
                    }
                    
                }

                    
                

                
            }

            
            return View(model);


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new DatabaseModels.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}