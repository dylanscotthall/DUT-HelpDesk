namespace DUT_HelpDesk.NewModels
{
    public class TicketViewModel
    {
        
        public string? Subject { get; set; }

        public string? QueryBody { get; set; }

        public IFormFile? File { get; set; }
        
    }
}
