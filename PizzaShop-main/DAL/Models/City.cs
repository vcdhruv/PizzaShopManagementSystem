using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class City
{
    public int CityId { get; set; }

    public int StateId { get; set; }

    public int CountryId { get; set; }

    public string CityName { get; set; } = null!;

    public bool? Isdeleted { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual State State { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
