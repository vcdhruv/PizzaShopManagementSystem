using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class ResetPasswordViewModel
{
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password Is Required")]
    public string NewPassword { get; set; }
    [Required(ErrorMessage = "Confirm Password Is Required")]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; }
}
