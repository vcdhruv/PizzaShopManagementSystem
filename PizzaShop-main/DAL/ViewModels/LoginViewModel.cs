using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email Is Required"),EmailAddress(ErrorMessage = "Please Provide Valid Email Address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password Is Required")]
    public string PasswordHash { get; set; }

    public bool RememberMe { get; set; }
}
