using System.ComponentModel.DataAnnotations;

namespace Restaurantproject.Models
{
    public class UserAccount
    {
        [Required(ErrorMessage = "the email requird")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "User Name")]
        public string User_Email { get; set; }
        [Required]
        [Compare("Confirm_Password", ErrorMessage = "please enter same password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The passwords do not match")]
        public string Confirm_Password { get; set; }
    }
}
