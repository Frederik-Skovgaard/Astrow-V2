using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Astrow_2._0.Pages.AdminPage
{
    public class UpdateUserModel : PageModel
    {

        private readonly IUserRepository _userRepository;

        public UpdateUserModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        #region Properties
        [BindProperty]
        public List<Users> UserList { get; set; }

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

        [BindProperty]
        public int ID { get; set; }

        [BindProperty]
        public bool Bool { get; set; }

        //------------------------ AbsRequest ------------------------

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


        /// <summary>
        /// On load fill out dropdown with all users
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
                    Abscenses = _userRepository.GettAbscenseTypeUserView();

                    //Fills dropdown with users 
                    UserList = _userRepository.ReadAllUsers();

                    Bool = false;

                    return Page();
                }
            }
        }

        /// <summary>
        /// When admin clicks on a name in the list, this filles out the fields with the user's info
        /// </summary>
        public void OnPostUser()
        {
            //Get abscense type
            Abscenses = _userRepository.GettAbscenseTypeUserView();

            if (ID != 0)
            {
                Users user = _userRepository.FindUser(ID);

                UserPersonalInfo person = _userRepository.FindUserInfo(ID);

                //Fill fields with user's info
                FirstName = person.FirstName;
                MiddleName = person.MiddleName;
                LastName = person.LastName;

                UserName = user.UserName;

                StartDate = user.StartDate.ToString("yyyy/MM/dd");
                EndDate = user.EndDate.ToString("yyyy/MM/dd");

                Role = user.Status.ToString();

                Bool = true;

                //Fills dropdown with users
                UserList = _userRepository.ReadAllUsers();
            }
            else
            {
                //Fills dropdown with users
                UserList = _userRepository.ReadAllUsers();
            }
        }

        /// <summary>
        /// Update User's infomation
        /// </summary>
        /// <param name="id"></param>
        public IActionResult OnPostUpdateUser(int id)
        {

            //Get user salt
            Users user = _userRepository.FindUser(id);

            if (Password != null)
            {
                //Use salt to hash the password
                string hashPass = _userRepository.GenerateHash(Password, user.Salt);

                //Fill user with new data
                user = new Users
                {
                    User_ID = id,
                    UserName = UserName,
                    Password = hashPass,
                    Status = Role,
                    StartDate = DateTime.Parse(StartDate),
                    EndDate = DateTime.Parse(EndDate)

                };
            }
            else
            {
                //Fill user with new data and old password
                user = new Users
                {
                    User_ID = id,
                    UserName = UserName,
                    Password = user.Password,
                    Status = Role,
                    StartDate = DateTime.Parse(StartDate),
                    EndDate = DateTime.Parse(EndDate)

                };
            }

            //Fill personal info with new data
            UserPersonalInfo userPersonalInfo = new UserPersonalInfo()
            {
                Name_ID = ID,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName
            };

            //Update users info
            _userRepository.UpdateUser(user);
            _userRepository.UpdateUserInfo(userPersonalInfo);

            Bool = false;

            return RedirectToPage("/AdminPage/Opdater-Bruger");
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
            return RedirectToPage("/AdminPage/Opdater-Bruger");
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
                        Date = date
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
                        SecDate = dateTwo
                    };

                    _userRepository.CreateRequestTwoDates(request);
                }
            }

            //Return to home page
            return RedirectToPage("/AdminPage/Opdater-Bruger");
        }


    }
}
