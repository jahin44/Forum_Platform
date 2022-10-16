using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models.Account
{
    public class ForgotPasswordModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}