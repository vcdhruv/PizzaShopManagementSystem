using System.Runtime.Serialization;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class UserRepository
{
    private readonly PizzaShopManagementSystemContext _context;
    public UserRepository(PizzaShopManagementSystemContext context)
    {
        _context = context;
    }

    #region Get all users
    public List<UserAuthentication> GetAllUsers()
    {
        return _context.UserAuthentications.Include(u => u.User).ThenInclude(u => u.Role).Where(u => u.Isdeleted == false).ToList();
    }
    #endregion

    #region Add user
    public async Task AddUser(User userModel)
    {
        _context.Add(userModel);
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Get user
    public async Task<UserAuthentication> GetUser(int UserID)
    {
        return await _context.UserAuthentications.Include(ua => ua.User).FirstOrDefaultAsync(u => u.UserId == UserID);
    }
    #endregion

    #region Update user
    public async Task UpdateUser(User userModel)
    {
        _context.Update(userModel);
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Delete user
    public async Task DeleteUser(int userid){
        User user = await _context.Users.FirstOrDefaultAsync(u=>u.UserId == userid);
        UserAuthentication userAuthentication = await _context.UserAuthentications.FirstOrDefaultAsync(u => u.UserId == userid);
        user.Isdeleted = true;
        userAuthentication.Isdeleted = true;
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Get countries
    public List<Country> GetCountries(){
        return _context.Countries.ToList();
    }
    #endregion

    #region Get States
    public List<State> GetStates(int CountryID){
        if (CountryID == -1)
        {
            return _context.States.ToList();
        }
        return _context.States.Where(state => state.CountryId == CountryID).ToList();
         
    }
    #endregion

    #region Get Cities
    public List<City> GetCities(int StateID){
        if (StateID == -1)
        {
            return _context.Cities.ToList();
        }
        return _context.Cities.Where(state => state.StateId == StateID).ToList();
         
    }
    #endregion
}
