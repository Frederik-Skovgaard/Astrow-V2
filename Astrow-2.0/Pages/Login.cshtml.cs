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
using Astrow_2._0.Model;
using Astrow_2._0.CustomExceptions;

namespace Astrow_2._0.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public LoginModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //-------------------Class-------------------------

        [BindProperty]
        public Users User { get; set; }

        [BindProperty]
        public LogedUser LogedUser { get; set; }


        //-------------------Login-------------------------

        [BindProperty, Required]
        public string UserName { get; set; }

        [BindProperty, Required]
        public string Password { get; set; }


        //-------------------Methods-----------------------


        public void OnGet()
        {

        }


        /// <summary>
        /// Login post
        /// </summary>
        public void OnPost()
        {
            byte[] s = Encoding.ASCII.GetBytes("Admin");

            User = new Users
            {
                UserName = "Admin",
                Password = s,
                Status = "Instructør",
                IsDeleted = false,
                Salt = "Astrow"
            };

            _userRepository.CreateUser(User);

            //Find salt
            User = _userRepository.FindByUserName(UserName);

            //Turn password and salt to byte
            byte[] password = Encoding.ASCII.GetBytes(Password);
            byte[] salt = Encoding.ASCII.GetBytes(User.Salt);

            //Use salt to hash the password
            byte[] hashPass = _userRepository.GenerateSaltedHash(password, salt);

            //Login the user and save the necesary data as LogedUser
            LogedUser = _userRepository.Login(UserName, password);

            //If LogedUser isen't null redirect to home page
            if (LogedUser != null)
            {
                RedirectToPage("/HomePage");
            }


            //Error throw if LogedUser is null which i coulden't be but never too much error handling
            throw new LoginException("An unexcpeted error happend... Please call for help..");
        }

       
    }
}
