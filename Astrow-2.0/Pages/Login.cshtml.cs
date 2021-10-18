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

namespace Astrow_2._0.Pages
{
    //TODO: Login that works with cookie
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;

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

            Users = new Users
            {
                UserName = "Admin",
                Password = s,
                Status = "Instruct�r",
                IsDeleted = false,
                Salt = "Astrow"
            };

            _userRepository.CreateUser(Users);

            //Find salt
            Users = _userRepository.FindByUserName(UserName);

            //Turn password and salt to byte
            byte[] password = Encoding.ASCII.GetBytes(Password);
            byte[] salt = Encoding.ASCII.GetBytes(Users.Salt);

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
