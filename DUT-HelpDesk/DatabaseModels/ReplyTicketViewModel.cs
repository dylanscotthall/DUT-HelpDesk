namespace DUT_HelpDesk.DatabaseModels
{
    public class ReplyTicketViewModel
    {
        //The file attachment of the reply.
        public IFormFile? file { get; set; }
        
        public Ticket ticket { get; set; }

        //The reply message.
        public string? Message { get; set; }
        
        //The email to forward ticket details to.
        public string? forwardEmail { get; set; }

        //The ID of the technician to assign the ticket to.
        public int? assignID { get; set; }

        public int id { get; set; }
    }
}
