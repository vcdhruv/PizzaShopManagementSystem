using System.Net.Http.Headers;
using BLL.Services;
using DAL.Models;
using DAL.Repository;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Management_System.BAL;
using Pizza_Shop_Management_System.Services;

namespace Pizza_Shop_Management_System.Controllers;

public class UserController : Controller
{

    #region Configuration Settings
    private readonly EmailService _emailService;
    private readonly UserService userService;

    #region Constructor
    public UserController(EmailService emailService, UserService _userService)
    {
        _emailService = emailService;
        userService = _userService;
    }
    #endregion

    #endregion

    #region User List Page
    public IActionResult Index()
    {
        var userListData = userService.GetAllUsers();
        return View(userListData);
    }
    #endregion

    #region Add New User Page
    public IActionResult AddNewUser()
    {
        return View();
    }
    #endregion

    #region Add New User Submit Method
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddNewUser(UserViewModel userViewModel)
    {
        await userService.AddUser(userViewModel);
        return RedirectToAction("Index");
    }
    #endregion

    #region Edit User Page
    public async Task<IActionResult> EditUser(int UserID)
    {
        UserViewModel editUserModel = await userService.GetUser(UserID);
        ViewData["Countries"] = new SelectList(GetCountries(), "CountryId", "CountryName");
        ViewData["States"] = new SelectList(GetStates(editUserModel.CountryID), "StateId", "StateName");
        ViewData["Cities"] = new SelectList(GetCities(editUserModel.StateID), "CityId", "CityName");
        return View(editUserModel);
    }
    #endregion

    #region Edit User Submit Method
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(int UserID, UserViewModel userViewModel)
    {
        await userService.UpdateUser(UserID, userViewModel);
        return RedirectToAction("Index");
    }
    #endregion

    #region Get Countries 
    [HttpGet]
    public List<Country> GetCountries()
    {
        var country = userService.GetCountries();
        return country;
    }
    #endregion

    #region Get States Of Selected Country
    [HttpGet]
    public List<State> GetStates(int countryID = -1)
    {
        return userService.GetStates(countryID);
    }
    #endregion

    #region Get Cities Of Selected State
    [HttpGet]
    public List<City> GetCities(int stateID)
    {
        return userService.GetCities(stateID);
    }
    #endregion

    #region Delete User
    [HttpPost]
    public async Task<IActionResult> DeleteUser(int userid)
    {
        await userService.DeleteUser(userid);
        return RedirectToAction("Index");
    }
    #endregion

}
