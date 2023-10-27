using System.ComponentModel.DataAnnotations;

namespace DUT_HelpDesk.DatabaseModels
{
    public class LoginViewModel
    {
        
        public int Id { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}
