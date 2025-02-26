using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public int CountryId { get; set; }

    public int StateId { get; set; }

    public int CityId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Zipcode { get; set; } = null!;

    public string ProfilePicture { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public DateTime Updatedat { get; set; }

    public bool Isdeleted { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual State State { get; set; } = null!;

    public virtual ICollection<UserAuthentication> UserAuthenticationCreatedbyNavigations { get; set; } = new List<UserAuthentication>();

    public virtual ICollection<UserAuthentication> UserAuthenticationUpdatedbyNavigations { get; set; } = new List<UserAuthentication>();

    public virtual ICollection<UserAuthentication> UserAuthenticationUsers { get; set; } = new List<UserAuthentication>();
}
