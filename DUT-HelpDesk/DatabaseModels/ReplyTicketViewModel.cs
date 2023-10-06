namespace DUT_HelpDesk.DatabaseModels
{
    public class ReplyTicketViewModel
    {
        
        public IFormFile? file { get; set; }

        public Ticket ticket { get; set; }

        public string? Message { get; set; }

        public int id { get; set; }
    }
}
