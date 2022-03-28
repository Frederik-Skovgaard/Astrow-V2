using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;
using System.Collections.Generic;

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

        //------------------------ Opret-Bruger Page ------------------------//

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

        //------------------------ AbsRequest ------------------------//

        [BindProperty]
        public List<AbscenseType> Abscenses { get; set; }

        [BindProperty]
        public string AbsCal { get; set; }

        [BindProperty]
        public string AbsCalTwo { get; set; }

        [BindProperty]
        public string AbsCalThree { get; set; }

        [BindProperty]
        public string Hour { get; set; }

        [BindProperty]
        public string HourTwo { get; set; }

        [BindProperty]
        public string HourThree { get; set; }

        [BindProperty]
        public int AbsenceType { get; set; }

        [BindProperty]
        public string AbscText { get; set; }

        [BindProperty]
        public int Bit { get; set; }

        #endregion

        //------------------------ Methods ------------------------//

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
                    //Get abscense type
                    Abscenses = _userRepository.GettAbscenseTypeUserView();

                    return Page();
                }
            }
        }


        /// <summary>
        /// Create User
        /// </summary>
        public void OnPost()
        {
            //Get abscense type
            Abscenses = _userRepository.GettAbscenseTypeUserView();

            //Generate salt
            string salt = _userRepository.CreateSalt(16);

            //Use salt to hash the password
            string hashPass = _userRepository.GenerateHash(Password, salt);

            UserPersonalInfo person = new UserPersonalInfo();

            if (MiddleName != null)
            {
                //Perosnal info
                person = new UserPersonalInfo()
                {
                    FirstName = _userRepository.FirstCharToUpper(FirstName),
                    MiddleName = _userRepository.FirstCharToUpper(MiddleName),
                    LastName = _userRepository.FirstCharToUpper(LastName),
                    FullName = $"{FirstName} {MiddleName} {LastName}"
                };
            }
            else
            {
                //Perosnal info
                person = new UserPersonalInfo()
                {
                    FirstName = _userRepository.FirstCharToUpper(FirstName),
                    MiddleName = MiddleName,
                    LastName = _userRepository.FirstCharToUpper(LastName),
                    FullName = $"{FirstName} {MiddleName} {LastName}"
                };
            }

           

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


        //------------------------ Nav Methods ------------------------//

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
        /// Get Start & End date for rendering years
        /// </summary>
        /// <returns></returns>
        public LogedUser GetDate()
        {
            string name = HttpContext.Session.GetString("_Username");

            LogedUser logedUser = new LogedUser();

            if (name != null)
            {
                return logedUser = new LogedUser
                {
                    UserName = name,
                    Status = HttpContext.Session.GetString("_Status"),
                    User_ID = (int)HttpContext.Session.GetInt32("_UserID"),
                    StartDate = DateTime.Parse(HttpContext.Session.GetString("_StartDate")),
                    EndDate = DateTime.Parse(HttpContext.Session.GetString("_EndDate"))
                };
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Method for requesting abscense
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostAbscenseRequest()
        {
            LogedUser log = GetDate();

            if (log != null)
            {
                if (Bit != 2)
                {
                    if (AbscText == null)
                    {
                        AbscText = "";
                    }

                    DateTime temp = DateTime.Parse(AbsCal);
                    DateTime HourDT = DateTime.Parse(Hour);
                    DateTime date = new DateTime(temp.Year, temp.Month, temp.Day, HourDT.Hour, HourDT.Minute, 0, 0);

                    Request request = new Request()
                    {
                        UserID = log.User_ID,
                        AbsID = AbsenceType,
                        Text = AbscText,
                        Date = date,
                        Answer = 3,
                        SecDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                    };

                    _userRepository.CreateRequest(request);
                }
                else
                {
                    if (AbscText == null)
                    {
                        AbscText = "";
                    }

                    DateTime CalOne = DateTime.Parse(AbsCalTwo);
                    DateTime CalTwo = DateTime.Parse(AbsCalThree);

                    DateTime HourOne = DateTime.Parse(HourTwo);
                    DateTime HourSec = DateTime.Parse(HourThree);

                    DateTime date = new DateTime(CalOne.Year, CalOne.Month, CalOne.Day, HourOne.Hour, HourOne.Minute, 0, 0);
                    DateTime dateTwo = new DateTime(CalTwo.Year, CalTwo.Month, CalTwo.Day, HourSec.Hour, HourSec.Minute, 0, 0);

                    Request request = new Request()
                    {
                        UserID = log.User_ID,
                        AbsID = AbsenceType,
                        Text = AbscText,
                        Date = date,
                        Answer = 3,
                        SecDate = dateTwo
                    };

                    _userRepository.CreateRequest(request);
                }
            }

            //Return to home page
            return RedirectToPage("/AdminPage/Opret-Bruger");
        }

    }    
}
