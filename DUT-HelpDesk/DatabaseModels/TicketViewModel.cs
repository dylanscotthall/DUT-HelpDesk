namespace DUT_HelpDesk.DatabaseModels
{
    public class TicketViewModel
    {
        
        public string? Subject { get; set; }

        public string? QueryBody { get; set; }

        public IFormFile? userFile { get; set; }
        
    }
}
