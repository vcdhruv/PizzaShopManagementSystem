using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Pizza_Shop_Management_System.Services;
using Pizza_Shop_Management_System.Services.Interfaces;
using DAL.ViewModels;

namespace Pizza_Shop_Management_System.Controllers
{

    public class UserAuthenticationController : Controller
    {
        #region Configuration Settings
        private readonly PizzaShopManagementSystemContext _context;
        private readonly EmailService _emailServices;
        private readonly IJwtService _jwtService;

        #region Constructor
        public UserAuthenticationController(PizzaShopManagementSystemContext context, EmailService emailServices, IJwtService jwtService)
        {
            _context = context;
            _emailServices = emailServices;
            _jwtService = jwtService;
        }
        #endregion

        #endregion

        #region Login Page
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            string Email = Request.Cookies["Email"];
            if (Email != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        #endregion

        #region Login Page Submit Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel login_details)
        {
            // If Email Or Password Is Null
            if (login_details.Email == null || login_details.PasswordHash == null)
            {
                return View();
            }
            if (ModelState.IsValid)
            {
                UserAuthentication user_auth_modal = new UserAuthentication();
                user_auth_modal.Email = login_details.Email;
                user_auth_modal.PasswordHash = login_details.PasswordHash;

                // Joining User Authentication , Users , Roles 
                var existingUser = _context.UserAuthentications.Include(a => a.User).ThenInclude(b => b.Role).Where(x => x.Email == user_auth_modal.Email).ToList();


                // If User Is Present
                if (existingUser != null)
                {

                    // Generate JWT Token
                    var jwt_Token = _jwtService.GenerateJwtToken(existingUser[0].User.FirstName + " " + existingUser[0].User.LastName, existingUser[0].Email, existingUser[0].User.Role.RoleName);

                    CookieOptions cookie = new CookieOptions()
                    {
                        Expires = DateTime.Now.AddDays(30)
                    };
                    Response.Cookies.Append("Jwt_Token", jwt_Token, cookie);

                    // If Remember Me Button Is Checked
                    if (login_details.RememberMe)
                    {
                        #region Cookie
                        Response.Cookies.Append("UserID", existingUser[0].User.UserId.ToString(), cookie);
                        Response.Cookies.Append("UserName", existingUser[0].User.UserName, cookie);
                        Response.Cookies.Append("Email", user_auth_modal.Email, cookie);
                        Response.Cookies.Append("Password", user_auth_modal.PasswordHash, cookie);
                        Response.Cookies.Append("RememberMe", "true", cookie);
                        #endregion
                    }

                    #region Session Details
                    // HttpContext.Session.SetString("JWT_Token",jwt_Token);
                    HttpContext.Session.SetString("Email", existingUser[0].Email);
                    HttpContext.Session.SetString("UserID", existingUser[0].UserId.ToString());
                    HttpContext.Session.SetString("UserName", existingUser[0].User.UserName);
                    HttpContext.Session.SetString("User_Profile_Picture", existingUser[0].User.ProfilePicture);
                    HttpContext.Session.SetString("RoleID", existingUser[0].User.RoleId.ToString());
                    HttpContext.Session.SetString("RoleName", existingUser[0].User.Role.RoleName);
                    #endregion

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {

                    // If Email Or Password Is Wrong Or Else Both Are Wrong
                    ViewBag.ErrorMsg = "No Such User Exists!";
                }
            }
            return View(login_details);
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Jwt_Token");
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        #endregion

        #region Forgot Password Page
        public IActionResult ForgotPassword()
        {
            return View();
        }
        #endregion

        #region Forgot Password Submit Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgot_password_vm)
        {
            if (forgot_password_vm.Email == null)
            {
                return View();
            }
            if (ModelState.IsValid)
            {
                UserAuthentication userAuthenticationModel = new UserAuthentication();
                userAuthenticationModel.Email = forgot_password_vm.Email;

                var existingEmail = _context.UserAuthentications.FirstOrDefault(ua => ua.Email == userAuthenticationModel.Email);
                if (existingEmail != null)
                {

                    ForgotPassword tokenDetails = new ForgotPassword();
                    ResetTokenGeneration tokens = new ResetTokenGeneration();
                    tokenDetails.Resettoken = tokens.TokenGenerator();
                    tokenDetails.Expirytime = DateTime.UtcNow.AddHours(24);
                    tokenDetails.UserAuthId = existingEmail.UserAuthId;

                    _context.Add(tokenDetails);
                    await _context.SaveChangesAsync();

                    var baseUrl = "http://localhost:5029/UserAuthentication/ResetPassword";
                    var resetLinkUrl = $"{baseUrl}?token={tokenDetails.Resettoken}&email={userAuthenticationModel.Email}";

                    string emailBody = $@"
                        <p>Pizza Shop,</p>
                        <span>Please click <a style ='color:blue' href ='{resetLinkUrl}'>here</a> for reset your account Password.
                        <p>If you encounter any issues or have any question, Please do not hesitate to contact our support team.</p>
                        <p><span style='color:yellow'>Important Note:</span>For security reasons, the link will expire in 24 hours. If you did not request a password reset, please ignore this email or contact our support team immediately.</p>
                    ";
                    await _emailServices.SendEmail(userAuthenticationModel.Email, "Reset Password", emailBody, true);

                }
                else
                {
                    ViewBag.ErrMsg = "No such email exists!";
                }
            }
            return View(forgot_password_vm);
        }
        #endregion

        #region Email Populate For Forgot Password
        [HttpPost]
        public string EmailPopulate(string? email)
        {
            TempData["Email"] = email;
            return email;
        }
        #endregion

        #region Reset Password Page
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            ResetPasswordViewModel email_vw = new ResetPasswordViewModel();
            email_vw.Email = email;
            var check = await _context.ForgotPasswords.FirstOrDefaultAsync(ua => ua.Resettoken == token);
            if (check.Expirytime > DateTime.Now)
            {
                return View(email_vw);
            }
            return NotFound("Link Is Expired");
        }
        #endregion

        #region Reset Password Submit Page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (resetPassword == null)
            {
                return View();
            }
            if (ModelState.IsValid)
            {
                // var password_update_email = email;

                var existingPaasword = _context.UserAuthentications.FirstOrDefaultAsync(ua => ua.Email == resetPassword.Email);
                existingPaasword.Result.PasswordHash = resetPassword.NewPassword;
                await _context.SaveChangesAsync();
                TempData["Success_Msg"] = "Password Changed Successfully...";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error_Msg"] = "Enter Valid Password";
            }
            return View();
        }
        #endregion

    }
}
