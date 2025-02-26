using System.Reflection;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class UserAuthenticationRepository
{
    private readonly PizzaShopManagementSystemContext _context;
    public UserAuthenticationRepository(PizzaShopManagementSystemContext context){
        _context = context;
    }

    #region Get user auth
    public async Task<UserAuthentication> GetUserAuth(int UserID){
        return await _context.UserAuthentications.FirstOrDefaultAsync(ua => ua.UserId == UserID);
    }
    #endregion

    #region Add user auth
    public async Task AddUserAuth(UserAuthentication userAuthModel){
        _context.Add(userAuthModel);
        await _context.SaveChangesAsync();        
    }
    #endregion

    #region Update user auth
    public async Task UpdateUserAuth(UserAuthentication userAuthentication){
        _context.Update(userAuthentication);
        await _context.SaveChangesAsync();
    }
    #endregion

    
}
