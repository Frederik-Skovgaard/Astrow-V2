using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.CustomExceptions;
using Microsoft.AspNetCore.Http;

namespace Astrow_2._0.Pages
{
    //TODO: Login that works with cookie
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public const string SessionUserID = "_UserID";
        public const string SessionUserName = "_Username";
        public const string sessionStatus = "_Status";
        public const string sessionStartDate = "_StartDate";
        public const string sessionEndDate = "_EndDate";


        public LoginModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //-------------------Class-------------------------

        [BindProperty]
        public Users Users { get; set; }

        [BindProperty]
        public LogedUser LogedUser { get; set; }


        //-------------------Login-------------------------

        [BindProperty, Required(ErrorMessage = "* Username is required")]
        public string UserName { get; set; }

        [BindProperty, Required(ErrorMessage = "* Password is required")]
        public string Password { get; set; }

        [BindProperty]
        public bool LoginMessage { get; set; }

        //-------------------Methods-----------------------


        public void OnGet()
        {
            HttpContext.Session.SetInt32(SessionUserID, 0);
            HttpContext.Session.SetString(SessionUserName, "");
            HttpContext.Session.SetString(sessionStatus, "");
            HttpContext.Session.SetString(sessionStartDate, "");
            HttpContext.Session.SetString(sessionEndDate, "");
        }


        /// <summary>
        /// Login post
        /// </summary>
        public IActionResult OnPost()
        {
            //Find salt
            Users = _userRepository.FindByUserName(UserName);

            if (Users.User_ID != 0)
            {
                //Generate salt
                string salt = Users.Salt;

                //Use salt to hash the password
                string hashPass = _userRepository.GenerateHash(Password, salt);

                //Login the user and save the necesary data as LogedUser
                LogedUser = _userRepository.Login(UserName, hashPass);

                //If LogedUser isen't null redirect to home page
                if (LogedUser.Status != null)
                {
                    HttpContext.Session.SetString(SessionUserName, LogedUser.UserName);
                    HttpContext.Session.SetInt32(SessionUserID, LogedUser.User_ID);
                    HttpContext.Session.SetString(sessionStatus, LogedUser.Status);
                    HttpContext.Session.SetString(sessionStartDate, LogedUser.StartDate.ToString());
                    HttpContext.Session.SetString(sessionEndDate, LogedUser.EndDate.ToString());

                    return RedirectToPage("/HomePage");
                }
                else
                {
                    LoginMessage = true;

                    return Page();
                }
            }
            else
            {

                LoginMessage = true;

                return Page();
            }


        }
    }
}
