namespace Pizza_Shop_Management_System.BAL;

public class CV
{
    private static IHttpContextAccessor _httpContextAccessor;

    #region Constructor
    static CV()
    {
        _httpContextAccessor = new HttpContextAccessor();
    }
    #endregion

    #region UserName
    public static string? UserName()
    {
        string? UserName = null;
        if (_httpContextAccessor.HttpContext.Session.GetString("UserName") != null)
        {
            UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName").ToString();
        }
        return UserName;
    }
    #endregion

    #region UserID
    public static int? UserID()
    {
        int? UserID = null;
        if (_httpContextAccessor.HttpContext.Session.GetString("UserID") != null)
        {
            UserID = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserID"));
        }
        return UserID;
    }
    #endregion

    #region Email
    public static string? Email()
    {
        string? Email = null;
        if (_httpContextAccessor.HttpContext.Session.GetString("Email") != null)
        {
            Email = _httpContextAccessor.HttpContext.Session.GetString("Email").ToString();
        }
        return Email;
    }
    #endregion

    #region RoleID
    public static int? RoleID()
    {
        int? RoleID = null;
        if (_httpContextAccessor.HttpContext.Session.GetString("RoleID") != null)
        {
            RoleID = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("RoleID"));
        }
        return RoleID;
    }
    #endregion

    #region RoleName
    public static string? RoleName()
    {
        string? RoleName = null;
        if (_httpContextAccessor.HttpContext.Session.GetString("RoleName") != null)
        {
            RoleName = _httpContextAccessor.HttpContext.Session.GetString("RoleName").ToString();
        }
        return RoleName;
    }
    #endregion

    #region Profile Picture
    public static string? User_Profile_Picture()
    {
        string? User_Profile_Picture = null;
        if (_httpContextAccessor.HttpContext.Session.GetString("User_Profile_Picture") != null)
        {
            User_Profile_Picture = _httpContextAccessor.HttpContext.Session.GetString("User_Profile_Picture").ToString();
        }
        return User_Profile_Picture;
    }
    #endregion

}
