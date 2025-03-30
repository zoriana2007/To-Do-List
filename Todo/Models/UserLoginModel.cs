using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class UserLoginModel
    {
        [Required]
        [Display(Name = "Username or Email")]
        public string UsernameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}