using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DAL.ViewModels;

public class UserViewModel
{
    public int? UserID { get; set; }
    public int CountryID { get; set; }
    public int StateID { get; set; }
    public int CityID { get; set; }
    public int RoleId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required,EmailAddress]
    public string Email { get; set; }
    [Required]
    public IFormFile ProfilePicture { get; set; }
    
    [Required,DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    // RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")
    public string Zipcode { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    // DataType(DataType.PhoneNumber),RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")
    public string PhoneNumber { get; set; }
    public string Status { get; set; } = "Active";
}
