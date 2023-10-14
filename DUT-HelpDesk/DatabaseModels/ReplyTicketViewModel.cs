using System.ComponentModel.DataAnnotations;

namespace DUT_HelpDesk.DatabaseModels

{
    public class ReplyTicketViewModel
    {
        //The file attachment of the reply.
        public IFormFile? file { get; set; }
        
        public Ticket ticket { get; set; }

        //The reply message.
        [Required(ErrorMessage = "Please enter a message.")]
        public string? Message { get; set; }
        
        //The email to forward ticket details to.
        public string? forwardEmail { get; set; }

        //The ID of the technician to assign the ticket to.
        public int? assignID { get; set; }

        //Stores whether the ticket is closed or not.
        public bool? isClosed { get; set; }

        public int id { get; set; }
    }
}
