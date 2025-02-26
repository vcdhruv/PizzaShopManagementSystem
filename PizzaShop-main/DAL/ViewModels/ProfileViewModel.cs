using DAL.Models;

namespace DAL.ViewModels;

public class ProfileViewModel
{
    public int CountryId { get; set; }
    public int StateId { get; set; }
    public int CityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; } = null!;
    public string Zipcode { get; set; } = null!;
}
