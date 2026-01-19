using System.ComponentModel.DataAnnotations;

namespace Restaurantproject.Models
{
    public class loginAcc
    {
        [Required(ErrorMessage = "User name or Email is Required..")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string password { get; set; }

    }
}
