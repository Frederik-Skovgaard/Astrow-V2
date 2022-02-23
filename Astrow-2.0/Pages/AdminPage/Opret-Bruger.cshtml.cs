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
                if (HttpContext.Session.GetString("_Status") != "InstructÝr")
                {
                    return RedirectToPage("/Home");
                }
                else
                {
                    UserPersonalInfo info = new UserPersonalInfo();

                    Users users = new Users();

                    for (int i = 0; i < 101; i++)
                    {
                        DateTime startDate = DateTime.Now;
                        DateTime endDate = startDate.AddYears(5);

                        //Generate salt
                        string salt = _userRepository.CreateSalt(16);

                        //Use salt to hash the password
                        string hashPass = _userRepository.GenerateHash($"Pass{i}", salt);

                        info = new UserPersonalInfo()
                        {
                            FirstName = $"Name {i}",
                            MiddleName = $"",
                            LastName = $"Last Name {i}",
                            FullName = $"Name {i} Last Name {i}"
                        };

                        users = new Users()
                        {
                            UserName = $"User {i}",
                            Password = hashPass,
                            Name_ID = info.Name_ID,
                            Status = "Elev",
                            StartDate = startDate,
                            EndDate = endDate,
                            Salt = salt.ToString(),
                            IsDeleted = false

                        };


                        //Create user with stored procedure
                        _userRepository.CreateUser(users, info);
                    }

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
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
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
        }
    }
}
