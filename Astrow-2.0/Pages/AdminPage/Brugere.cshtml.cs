using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

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
        public List<AbscenseType> AbscenseType { get; set; }

        [BindProperty]
        public string Abscense { get; set; }

        [BindProperty]
        public string AbscenseText { get; set; }

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
                    //Get abscense type
                    AbscenseType = _userRepository.GetAllAbscenseType();

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
        public IActionResult OnPostDelete()
        {
            //Check user of as deleted in database
            _userRepository.DeleteUser(ID);


            return RedirectToPage("/AdminPage/Brugere");
        }

        /// <summary>
        /// Method for marking down a user as sick
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostSickNotice()
        {
            if (ID != 0)
            {
                if (Abscense != "1")
                {
                    if (AbscenseText == null)
                    {
                        AbscenseText = "";
                    }

                    Days day = _userRepository.FindDay(DateTime.Now.Date, ID);

                    _userRepository.UpdateAbsence(AbscenseText, day.Days_ID);
                    _userRepository.UpdateAbsencseType(Convert.ToInt32(Abscense), day.Days_ID);

                }
            }
            

            return RedirectToPage("/AdminPage/Brugere");
        }

        /// <summary>
        /// Method for marking down a user as illegaly abscent 
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostIllegalNotice()
        {
            if (ID != 0)
            {
                Days day = _userRepository.FindDay(DateTime.Now.Date, ID);

                AbscenseType abscense = _userRepository.FindAbscenseByText("Ulovligt fravær");

                _userRepository.UpdateAbsencseType(abscense.ID, day.Days_ID);
            }

            

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
