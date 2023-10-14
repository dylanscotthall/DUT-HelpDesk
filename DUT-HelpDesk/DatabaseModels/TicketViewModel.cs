
using System.ComponentModel.DataAnnotations;

namespace DUT_HelpDesk.DatabaseModels
{
    public class TicketViewModel
    {
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? QueryBody { get; set; }

        public IFormFile? File { get; set; }
        
    }
}
