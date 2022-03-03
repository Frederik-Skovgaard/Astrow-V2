using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;

namespace Astrow_2._0.Pages.AdminPage
{
    public class CreateUsersModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public CreateUsersModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #region Properties
        [BindProperty, MaxLength(20)]
        public string UserName { get; set; }

        [BindProperty, MaxLength(30)]
        public string FirstName { get; set; }

        [BindProperty, MaxLength(30)]
        public string MiddleName { get; set; }

        [BindProperty, MaxLength(30)]
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
        public string FirstNameVal { get; set; }

        [BindProperty]
        public string MiddleNameVal { get; set; }

        [BindProperty]
        public string LastNameVal { get; set; }

        [BindProperty]
        public string UserNameVal { get; set; }

        [BindProperty]
        public string StartVal { get; set; }

        [BindProperty]
        public string EndVal { get; set; }

        #endregion

        /// <summary>
        /// Check if users has rights to be on side
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
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
                    return Page();
                }
            }
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
            return RedirectToPage("/AdminPage/Opret-Bruger");
        }


        /// <summary>
        /// Create User
        /// </summary>
        public void OnPost()
        {
            
            //Generate salt
            string salt = _userRepository.CreateSalt(16);

            //Use salt to hash the password
            string hashPass = _userRepository.GenerateHash(Password, salt);

            //Perosnal info
            UserPersonalInfo person = new UserPersonalInfo()
            {
                FirstName = _userRepository.FirstCharToUpper(FirstName),
                MiddleName = _userRepository.FirstCharToUpper(MiddleName),
                LastName = _userRepository.FirstCharToUpper(LastName),
                FullName = $"{FirstName} {MiddleName} {LastName}"
            };

            //User info
            Users users = new Users()
            {
                UserName = UserName,
                Password = hashPass,
                Name_ID = person.Name_ID,
                Status = Role,
                StartDate = DateTime.Parse(StartDate),
                EndDate = DateTime.Parse(EndDate),
                Salt = salt.ToString(),
                IsDeleted = false                
            };

            //Create user with stored procedure
            _userRepository.CreateUser(users, person);

            FirstNameVal = "";
            MiddleNameVal = "";
            LastNameVal = "";
            UserNameVal = "";
            StartVal = "";
            EndVal = "";


        }

        
    }    
}
