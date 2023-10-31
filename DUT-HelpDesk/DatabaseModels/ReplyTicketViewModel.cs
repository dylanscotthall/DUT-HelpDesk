using System.ComponentModel.DataAnnotations;

namespace DUT_HelpDesk.DatabaseModels{


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

        //Stores whether the ticket is closed or not.
        public bool? isClosed { get; set; }

        public int id { get; set; }

        public string? subject { get; set; }

        public string? body { get; set; }

        public DateTime? openDate { get; set; }

        public string? priority { get; set; }

        [Required]
        public Priority? Priority { get; set; }

        [Required (ErrorMessage = "Please select a rating.")]
        [Range(1, 5, ErrorMessage = "Please select a rating between 1 and 5.")]
        public int Rating { get; set; }

        [Required]
        public string? Comments { get; set;}
    }
}
