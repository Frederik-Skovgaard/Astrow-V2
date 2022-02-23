using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Astrow_2._0.Pages.AdminPage
{
    public class DeleteUserModel : PageModel
    {

        private readonly IUserRepository _userRepository;

        public DeleteUserModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public List<Users> UserList { get; set; }

        [BindProperty]
        public List<PersonalInfo> People { get; set; }

        [BindProperty]
        public int ID { get; set; }

        /// <summary>
        /// Check if users has rights to be on side
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

                    People = new List<PersonalInfo>();

                    foreach (Users item in UserList)
                    {
                        UserPersonalInfo person = _userRepository.FindUserInfo(item.User_ID);

                        PersonalInfo personalInfo = new PersonalInfo()
                        {
                            ID = item.User_ID,
                            UserName = item.UserName,
                            Status = item.Status,
                            FirstName = person.FirstName,
                            MiddleName = person.MiddleName,
                            LastName = person.LastName
                        };

                        People.Add(personalInfo);
                    }

                    return Page();
                }
            }
            
        }

        /// <summary>
        /// Method for deleting users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            //Check user of as deleted in database
            _userRepository.DeleteUser(ID);


            //Popup message for succes
            ViewData["Message"] = string.Format("Bruger blev slettet...");

            return RedirectToPage("/AdminPage/Brugere");
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
            return RedirectToPage("/AdminPage/Slet-Bruger");
        }
    }
}
