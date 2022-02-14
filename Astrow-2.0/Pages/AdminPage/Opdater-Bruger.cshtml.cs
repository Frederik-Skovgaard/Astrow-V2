using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Astrow_2._0.Pages.AdminPage
{
    public class UpdateUserModel : PageModel
    {

        private readonly IUserRepository _userRepository;

        public UpdateUserModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        #region Properties
        [BindProperty]
        public List<Users> UserList { get; set; }

        [BindProperty]
        public string UserName { get; set; }


        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string MiddleName { get; set; }

        [BindProperty]
        public string LastName { get; set; }


        [BindProperty]
        public string Password { get; set; }


        [BindProperty]
        public string Role { get; set; }

        [BindProperty]
        public string StartDate { get; set; }

        [BindProperty]
        public string EndDate { get; set; }

        [BindProperty]
        public int ID { get; set; }
        #endregion


        /// <summary>
        /// On load fill out dropdown with all users
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            //Check if user has "Instructør" rights
            if (HttpContext.Session.GetInt32("_UserID") == 0)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                if (HttpContext.Session.GetString("_Status") != "Instructør")
                {
                    return RedirectToPage("/Home");
                }
                else
                {
                    //Fills dropdown with users 
                    UserList = _userRepository.ReadAllUsers();

                    return Page();
                }
            }
        }

        /// <summary>
        /// When admin clicks on a name in the list, this filles out the fields with the user's info
        /// </summary>
        public void OnPostUser()
        {
            if (ID != 0)
            {
                Users user = _userRepository.FindUser(ID);

                UserPersonalInfo person = _userRepository.FindUserInfo(ID);

                //Fill fields with user's info
                FirstName = person.FirstName;
                MiddleName = person.MiddleName;
                LastName = person.LastName;

                UserName = user.UserName;

                StartDate = user.StartDate.ToString("yyyy/MM/dd");
                EndDate = user.EndDate.ToString("yyyy/MM/dd");

                Role = user.Status.ToString();

                //Fills dropdown with users
                UserList = _userRepository.ReadAllUsers();
            }
            else
            {
                //Fills dropdown with users
                UserList = _userRepository.ReadAllUsers();
            }
        }

        /// <summary>
        /// Update User's infomation
        /// </summary>
        /// <param name="id"></param>
        public void OnPostUpdateUser(int id)
        {
            //Get user salt
            Users user = _userRepository.FindUser(id);

            if (Password != null)
            {
                //Use salt to hash the password
                string hashPass = _userRepository.GenerateHash(Password, user.Salt);

                //Fill user with new data
                user = new Users
                {
                    User_ID = id,
                    UserName = UserName,
                    Password = hashPass,
                    Status = Role,
                    StartDate = DateTime.Parse(StartDate),
                    EndDate = DateTime.Parse(EndDate)
                    
                };
            }
            else
            {
                //Fill user with new data and old password
                user = new Users
                {
                    User_ID = id,
                    UserName = UserName,
                    Password = user.Password,
                    Status = Role,
                    StartDate = DateTime.Parse(StartDate),
                    EndDate = DateTime.Parse(EndDate)
                    
                };
            }

            //Fill personal info with new data
            UserPersonalInfo userPersonalInfo = new UserPersonalInfo()
            {
                Name_ID = ID,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName
            };

            //Update users info
            _userRepository.UpdateUser(user);
            _userRepository.UpdateUserInfo(userPersonalInfo);

            //Popup message for succes
            ViewData["Message"] = string.Format("Bruger blev opdateret...");
        }


        /// <summary>
        /// Method for clocking in and out
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostRegistrering()
        {
            //User ID
            int id = (int)HttpContext.Session.GetInt32("_UserID");

            //Method for registry
            _userRepository.Registrer(id);

            //Return to home page
            return RedirectToPage("/AdminPage/Opdater-Bruger");
        }


    }
}
