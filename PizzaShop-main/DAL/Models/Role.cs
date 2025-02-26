﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public bool? Isdeleted { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
