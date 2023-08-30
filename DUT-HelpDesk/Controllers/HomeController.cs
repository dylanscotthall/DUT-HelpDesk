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
        public async Task<IActionResult> CreateTicket(TicketViewModel model)
        {
           
            
              
            if (ModelState.IsValid)
            {
                // Access the uploaded file using model.File


                //await db.Tickets.AddAsync(ticket);
                Console.WriteLine($"{model.Subject}");
                    // Process the uploaded file
                    // For example, you can save it to disk or store in a database
                

                // Process other form data (Subject, QueryBody) here

                // Redirect or return appropriate view
            }

            // If ModelState is not valid, return the view with validation errors
            return View(model);


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
            return View(new DatabaseModels.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}