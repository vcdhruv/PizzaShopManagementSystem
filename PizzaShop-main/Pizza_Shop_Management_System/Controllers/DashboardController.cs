using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using DAL.ViewModels;
using Pizza_Shop_Management_System.BAL;

namespace Pizza_Shop_Management_System.Controllers
{
    
    public class DashboardController : Controller
    {

        #region Configuration Settings
        private readonly PizzaShopManagementSystemContext _context;
        private readonly UserController users;


        public DashboardController(PizzaShopManagementSystemContext context, UserController users)
        {
            _context = context;
            this.users = users;

        }
        #endregion

        #region Index 
        // [Authorize]
        // GET: Users
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("Email")== null && Request.Cookies["Email"]== null){
                return RedirectToAction("Index","UserAuthentication");
            }
            
            return View();
        }
        #endregion

        #region My Profile
        public async Task<IActionResult> MyProfile()
        {
            string? email = HttpContext.Session.GetString("Email") == null ? Request.Cookies["Email"] : HttpContext.Session.GetString("Email");
            var status = await _context.UserAuthentications.Include(ua => ua.User).ThenInclude(u => u.Country).ThenInclude(u => u.States).ThenInclude(u => u.Cities).FirstOrDefaultAsync(u => u.Email == email);
           

            ProfileViewModel profile_details = new ProfileViewModel();
            profile_details.Address = status!.User.Address;
            profile_details.PhoneNumber = status.User.PhoneNumber;
            profile_details.FirstName = status.User.FirstName;
            profile_details.LastName = status.User.LastName;
            profile_details.UserName = status.User.UserName;
            profile_details.Zipcode = status.User.Zipcode;
            profile_details.CountryId = status.User.CountryId;
            profile_details.StateId = status.User.StateId;
            profile_details.CityId = status.User.CityId;

            ViewData["Countries"] = new SelectList(this.users.GetCountries(), "CountryId","CountryName");
            ViewData["States"] = new SelectList(this.users.GetStates(status.User.CountryId), "StateId","StateName");
            ViewData["Cities"] = new SelectList(this.users.GetCities(status.User.StateId), "CityId","CityName");

            return View(profile_details);
        }
        #endregion

        #region My Profile Submit Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyProfile(ProfileViewModel status)
        {
            string? email = HttpContext.Session.GetString("Email") == null ? Request.Cookies["Email"] : HttpContext.Session.GetString("Email");
            var profile_details = await _context.UserAuthentications.Include(ua => ua.User).ThenInclude(u => u.Country).ThenInclude(u => u.States).ThenInclude(u => u.Cities).FirstOrDefaultAsync(u => u.Email == email);
                    
            if(profile_details == null){
                return View();
            }
            if(ModelState.IsValid){                  
                 
                profile_details.User.Address = status.Address;
                profile_details.User.PhoneNumber = status.PhoneNumber;
                profile_details.User.FirstName = status.FirstName;
                profile_details.User.LastName = status.LastName;
                profile_details.User.UserName = status.UserName;
                profile_details.User.Zipcode = status.Zipcode;
                profile_details.User.CountryId = status.CountryId;
                profile_details.User.StateId = status.StateId;
                profile_details.User.CityId = status.CityId;
                _context.Update(profile_details);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index","Users");
        }
        #endregion

        #region Change Password
        public IActionResult ChangePassword()
        {
            return View();
        } 
        #endregion

        #region Change Password Submit Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel cng_pass_vm)
        {
            string? email = CV.Email() == null ? Request.Cookies["Email"] : CV.Email();
            var existingEmail = await _context.UserAuthentications.FirstOrDefaultAsync(u => u.Email == email);
            var existingPass = await _context.UserAuthentications.FirstOrDefaultAsync(u => u.PasswordHash == cng_pass_vm.CurrentPassword);
            if(existingEmail != null && existingPass != null){
                if(ModelState.IsValid){
                    existingEmail.PasswordHash = cng_pass_vm.NewPassword;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            TempData["Error_Msg"] = "Some Error Occurred , Try Again!";
            return View(cng_pass_vm);
        }
        #endregion
    }
}
