using DUT_HelpDesk.DatabaseModels;
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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public async Task<IActionResult> CreateTicket(TicketViewModel ticket)
        {
           
            
              
            if (ModelState.IsValid)
            {
                //get current user


                //add ticket to db
                //get ticet id for attachment
                if(ticket.File != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await ticket.File.CopyToAsync(stream);

                        stream.Seek(0, SeekOrigin.Begin);

                        var uploadedFile = new Attachment()
                        {
                            //get ticketid
                            FileName = ticket.File.Name,
                            FileContent = stream.ToArray(),
                            ContentType = ticket.File.ContentType
                        };

                        db.Attachments.Add(uploadedFile);
                    }
                }

                    
                

                
            }

            
            return View(ticket);


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new DatabaseModels.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}