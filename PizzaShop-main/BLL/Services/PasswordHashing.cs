using System.Security.Cryptography;
using System.Text;
using Pizza_Shop_Management_System.Services.Interfaces;

namespace Pizza_Shop_Management_System.Services;

public class PasswordHashing
{
    #region Encrypt function
    public string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    #endregion

    #region Decrypt function
    public string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    #endregion
}
