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

        [BindProperty, Required]
        public string UserName { get; set; }

        [BindProperty, Required]
        public string Password { get; set; }

        [BindProperty]
        public bool LoginMessage { get; set; }

        //-------------------Methods-----------------------


        public void OnGet()
        {
            HttpContext.Session.SetInt32(SessionUserID, 0);
            HttpContext.Session.SetString(SessionUserName, "");
            HttpContext.Session.SetString(sessionStatus, "");
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
                //Turn password and salt to byte
                byte[] password = Encoding.ASCII.GetBytes(Password);
                byte[] salt = Encoding.ASCII.GetBytes(Users.Salt);

                //Use salt to hash the password
                string hashPass = _userRepository.GenerateSaltedHash(password, salt);

                //Login the user and save the necesary data as LogedUser
                LogedUser = _userRepository.Login(UserName, hashPass);

                //If LogedUser isen't null redirect to home page
                if (LogedUser != null)
                {
                    HttpContext.Session.SetString(SessionUserName, LogedUser.UserName);
                    HttpContext.Session.SetInt32(SessionUserID, LogedUser.User_ID);
                    HttpContext.Session.SetString(sessionStatus, LogedUser.Status);
                    return RedirectToPage("/HomePage");
                }
                else
                {
                    throw new LoginException("Mesa nosa understand why it dosen't worken");
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
