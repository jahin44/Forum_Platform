using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Forum.Web.Models.Account
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}
