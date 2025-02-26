using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class UserAuthentication
{
    public int UserAuthId { get; set; }

    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Status { get; set; }

    public int Createdby { get; set; }

    public int? Updatedby { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Updatedat { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual ICollection<ForgotPassword> ForgotPasswords { get; set; } = new List<ForgotPassword>();

    public virtual User? UpdatedbyNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
