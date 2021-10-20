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
        public TimeCard TimeCard { get; set; }

        [BindProperty]
        public Days Days { get; set; }

        public UserPersonalInfo UserInfo { get; set; }

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
            byte[] v = Encoding.ASCII.GetBytes("Astrow");
            string p = _userRepository.GenerateSaltedHash(s, v);

            Users = new Users
            {
                UserName = "Admin",
                Password = p,
                Status = "Instructør",
                IsDeleted = false,
                Salt = "Astrow",
                StartDate = new DateTime(2020, 8, 13),
                EndDate = new DateTime(2025, 1, 1)
            };

            UserInfo = new UserPersonalInfo
            {
                FirstName = "Adam",
                MiddleName = "The",
                LastName = "Admin",
                FullName = "Adam The Admin"
            };

            Days = new Days
            {
                Date = new DateTime(1944, 6, 6, 0, 0, 0),
                AbsenceDate = new DateTime(1944, 6, 6, 0, 0, 0),
                AbscenceText = "",
                StartDay = new DateTime(1944, 6, 6, 0, 0, 0),
                EndDay = new DateTime(1944, 6, 6, 0, 0, 0),
                Saldo = new DateTime(1944, 6, 6, 0, 0, 0),
                Flex = new DateTime(1944, 6, 6, 0, 0, 0)
            };

            TimeCard = new TimeCard();

            UserName = Users.UserName;

            _userRepository.CreateUser(Users, Days, UserInfo);

            //Find salt
            Users = _userRepository.FindByUserName(UserName);

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
                RedirectToPage("/HomePage");
            }


            //Error throw if LogedUser is null which i coulden't be but never too much error handling
            throw new LoginException("An unexcpeted error happend... Please call for help..");
        }

        


       
    }
}
