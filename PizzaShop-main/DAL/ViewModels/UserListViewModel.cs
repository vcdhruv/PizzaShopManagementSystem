namespace DAL.ViewModels;

public class UserListViewModel
{
    public int UserID { get; set; }
    public string FirstName { get; set; }
    public string ProfilePicture { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }
    public bool IsDeleted { get; set; }
    public string Status { get; set; }
}
