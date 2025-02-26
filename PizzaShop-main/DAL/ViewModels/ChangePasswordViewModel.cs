using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Current Password Is Required")]
    public string CurrentPassword { get; set; }
    [Required(ErrorMessage = "New Password Is Required")]
    public string NewPassword { get; set; }
    [Required(ErrorMessage = "Confirm New Password Is Required"),Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; }
}
