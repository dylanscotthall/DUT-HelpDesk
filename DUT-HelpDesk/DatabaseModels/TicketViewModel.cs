using System.ComponentModel.DataAnnotations;

namespace DUT_HelpDesk.DatabaseModels
{
    public enum Priority
    {
        Low,
        Normal,
        High
    }

    public class TicketViewModel
    {
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? QueryBody { get; set; }

        [Required]
        public Priority? Priority { get; set; }

        public IFormFile? File { get; set; }
    }
}
