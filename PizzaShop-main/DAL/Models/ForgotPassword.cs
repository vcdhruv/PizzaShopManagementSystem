using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ForgotPassword
{
    public int ForgotpasswordId { get; set; }

    public int UserAuthId { get; set; }

    public string Resettoken { get; set; } = null!;

    public DateTime Expirytime { get; set; }

    public virtual UserAuthentication UserAuth { get; set; } = null!;
}
