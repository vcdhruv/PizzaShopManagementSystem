using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email Address Is Required")]
    public string Email { get; set; }
}
