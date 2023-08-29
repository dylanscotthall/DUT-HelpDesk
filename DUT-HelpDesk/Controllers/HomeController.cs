using DUT_HelpDesk.Model;
using DUT_HelpDesk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace DUT_HelpDesk.Controllers
{
    public class HomeController : Controller
    {
        private NewModels.DutHelpdeskdbContext db = new NewModels.DutHelpdeskdbContext();
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
            NewModels.Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return StatusCode(400);
            }
            return View(ticket);
        }

        public IActionResult CreateTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(NewModels.TicketViewModel ticket)
        {
            if(ticket != null)
            {
                //await db.Tickets.AddAsync(ticket);
                Console.WriteLine($"File content type: {ticket.File.ContentType}");
                return RedirectToAction("UserTicket");
            }
            else
            {
                return View(ticket);
            }

        }

        [HttpPost]
        public async Task<ActionResult> AddAttachment(IFormFile file)
        {
            Console.WriteLine($"File content type: {file.ContentType}");
            if (file != null && file.Length > 0)
            {

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    stream.Seek(0, SeekOrigin.Begin);

                    using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string content = await streamReader.ReadToEndAsync();



                    }
                    //var uploadedFile = new NewModels.File
                    //{
                       // FileName = file.FileName,
                       // FileContent = stream.ToArray(),

                    //};

                    //NewModels.TestFiles2Context db = new NewModels.TestFiles2Context();
                    //uploadedFile.FileId = 8;
                    //db.Files.Add(uploadedFile);
                    //await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("CreateTicket", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}