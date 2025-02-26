using DAL.Models;
using DAL.Repository;
using DAL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Pizza_Shop_Management_System.Services;

namespace BLL.Services;

public class UserService
{
    private readonly UserRepository userRepository;
    private readonly UserAuthenticationRepository userAuthenticationRepository;
    private readonly IWebHostEnvironment _env;
    private readonly PasswordHashing hashing;


    public UserService(UserRepository _userRepository, PasswordHashing _hashing, IWebHostEnvironment env, UserAuthenticationRepository _userAuthenticationRepository)
    {
        userRepository = _userRepository;
        _env = env;
        hashing = _hashing;
        userAuthenticationRepository = _userAuthenticationRepository;
    }

    #region Get all users
    public List<UserListViewModel> GetAllUsers()
    {
        List<UserAuthentication> userAuthentications = userRepository.GetAllUsers();
        List<UserListViewModel> userListData = new List<UserListViewModel>();
        foreach (var users in userAuthentications)
        {
            UserListViewModel userListViewModel = new()
            {
                UserID = users.UserId,
                Email = users.Email,
                FirstName = users.User.FirstName,
                LastName = users.User.LastName,
                PhoneNumber = users.User.PhoneNumber,
                Role = users.User.Role.RoleName,
                IsDeleted = users.Isdeleted,
                Status = users.Status,
                ProfilePicture = users.User.ProfilePicture
            };
            userListData.Add(userListViewModel);
        }
        return userListData;
    }
    #endregion

    #region Add user
    public async Task AddUser(UserViewModel userViewModel)
    {
        #region Upload File With IFormFile
        string filename = "";
        string filepath = "";
        if (userViewModel.ProfilePicture != null)
        {
            string folder = Path.Combine(_env.WebRootPath, "Profile_pictures");
            filename = Guid.NewGuid().ToString() + "_" + userViewModel.ProfilePicture.FileName; // Guid means Global unique identifier -> It will generate random character
            filepath = Path.Combine(folder, filename);
            userViewModel.ProfilePicture.CopyTo(new FileStream(filepath, FileMode.Create));
        }
        #endregion

        #region Add User To User Table
        User userModel = new User();
        userModel.RoleId = userViewModel.RoleId;
        userModel.CountryId = userViewModel.CountryID;
        userModel.StateId = userViewModel.StateID;
        userModel.CityId = userViewModel.CityID;
        userModel.UserName = userViewModel.UserName;
        userModel.FirstName = userViewModel.FirstName;
        userModel.LastName = userViewModel.LastName;
        userModel.Address = userViewModel.Address;
        userModel.ProfilePicture = "Profile_pictures\\" + filename; // storing image path to database
        userModel.Zipcode = userViewModel.Zipcode;
        userModel.PhoneNumber = userViewModel.PhoneNumber;
        #endregion
        await userRepository.AddUser(userModel);

        #region Add User To User-Authentication Table
        UserAuthentication userAuthModel = new UserAuthentication();
        userAuthModel.UserId = userModel.UserId;
        userAuthModel.Email = userViewModel.Email;
        userAuthModel.Createdby = 1;
        userAuthModel.PasswordHash = hashing.Base64Encode(userViewModel.Password);
        userAuthModel.Status = userViewModel.Status;
        await userAuthenticationRepository.AddUserAuth(userAuthModel);
        #endregion
        // string emailBody = $@"
        //     <p>Welcome to Pizza shop</p>
        //     <p>Please find the below for login into your account</p>
        //     <div style='border:2px solid black'>
        //         <h1>Login Details:</h1>
        //         <h2>Username : '{userViewModel.Email}'</h2>
        //         <h2>Temporary Password : {userViewModel.Password} </h2>
        //     </div>
        //     <span>If you encounter any issues or have any question, please do not hesitate to contact our support team.</span>
        // ";

        // await _emailService.SendEmail(userViewModel.Email, "User Created", emailBody, true);
    }
    #endregion

    #region Get user
    public async Task<UserViewModel> GetUser(int UserID)
    {
        UserAuthentication user = await userRepository.GetUser(UserID);
        UserViewModel editUserModel = new()
        {
            Address = user.User.Address,
            CityID = user.User.CityId,
            CountryID = user.User.CountryId,
            Email = user.Email,
            FirstName = user.User.FirstName,
            LastName = user.User.LastName,
            Password = user.PasswordHash,
            PhoneNumber = user.User.PhoneNumber,
            // ProfilePicture = ConvertFilePathToIFormFile(user.User.ProfilePicture.ToString()),
            RoleId = user.User.RoleId,
            StateID = user.User.StateId,
            Status = user.Status,
            UserName = user.User.UserName,
            Zipcode = user.User.Zipcode,
            UserID = user.UserId,
        };
        return editUserModel;
    }
    #endregion

    #region Update User
    public async Task UpdateUser(int UserID, UserViewModel userViewModel){
        #region Edit File With IFormFile
        string filename = "";
        string filepath = "";
        if (userViewModel.ProfilePicture != null)
        {
            string folder = Path.Combine(_env.WebRootPath, "Profile_pictures");
            filename =  userViewModel.ProfilePicture.FileName; // Guid means Global unique identifier -> It will generate random character
            filepath = Path.Combine(folder, filename);
            userViewModel.ProfilePicture.CopyTo(new FileStream(filepath, FileMode.Create));
        }
        #endregion

        User userModel = new User();
        userModel.UserId = UserID;
        userModel.RoleId = userViewModel.RoleId;
        userModel.CountryId = userViewModel.CountryID;
        userModel.StateId = userViewModel.StateID;
        userModel.CityId = userViewModel.CityID;
        userModel.UserName = userViewModel.UserName;
        userModel.FirstName = userViewModel.FirstName;
        userModel.LastName = userViewModel.LastName;
        userModel.Address = userViewModel.Address;
        userModel.ProfilePicture = "Profile_pictures\\" + filename ;
        userModel.Zipcode = userViewModel.Zipcode;
        userModel.PhoneNumber = userViewModel.PhoneNumber;
        await userRepository.UpdateUser(userModel);        

        var existingUser = await userAuthenticationRepository.GetUserAuth(UserID);
        existingUser.Status = userViewModel.Status;
        existingUser.Updatedby = 1;
        existingUser.Updatedat = DateTime.UtcNow;
        await userAuthenticationRepository.UpdateUserAuth(existingUser);
    }
    #endregion

    #region Delete user
    public async Task DeleteUser(int userid){
        await userRepository.DeleteUser(userid);
    }
    #endregion

    #region Get countries
    public List<Country> GetCountries(){
        return userRepository.GetCountries();
    }
    #endregion

    #region Get states
    public List<State> GetStates(int CountryID){
        return userRepository.GetStates(CountryID);
    }    
    #endregion

    #region Get Cities
    public List<City> GetCities(int StateID){
        return userRepository.GetCities(StateID);
    }    
    #endregion
}
