using System.ComponentModel.DataAnnotations;

namespace DUT_HelpDesk.DatabaseModels
{
    public class RegisterViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
