using System.ComponentModel.DataAnnotations;

namespace SongsProject.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
