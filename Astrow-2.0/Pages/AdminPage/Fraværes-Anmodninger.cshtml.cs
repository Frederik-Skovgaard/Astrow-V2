using Astrow_2._0.Model.Items;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;

namespace Astrow_2._0.Pages.AdminPage
{
    public class Fraværes_AnmodningerModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public Fraværes_AnmodningerModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        //------------------------ RequestPage ------------------------//


        [BindProperty]
        public List<Request> Requests { get; set; }

        [BindProperty]
        public List<PersonalInfo> People { get; set; }

        [BindProperty]
        public int ID { get; set; }


        //------------------------ AbsRequest ------------------------//

        [BindProperty]
        public List<AbscenseType> AbscensesRequest { get; set; }

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


        //------------------------ Method ------------------------//

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
                    AbscensesRequest = _userRepository.GettAbscenseTypeUserView();

                    //Get all requests
                    Requests = _userRepository.GetRequests();
                    Requests = Requests.FindAll(x => x.Answer == 3);

                    //Fills dropdown with users 
                    List<Users> UserList = _userRepository.ReadAllUsers();

                    People = _userRepository.GetPeople();

                    return Page();
                }
            }

        }

        public IActionResult OnPostDeny()
        {
            //Deny request
            _userRepository.UpdateRequestAnswered(ID, 0);

            //Return to page
            return RedirectToPage("/AdminPage/Fraværes-Anmodninger");
        }

        public IActionResult OnPostAccept()
        {
            Request req = _userRepository.FindRequest(ID);

            //Update TimeCard
            switch (req.AbsID)
            {
                case 2:
                    UpdateAbscens(req.AbsID);
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    UpdateAbscens(req.AbsID);
                    break;
                case 6:
                    UpdateAbscens(req.AbsID);
                    break;
                case 7:
                    UpdateAbscens(req.AbsID);
                    break;
                case 8:
                    UpdateAbscens(req.AbsID);
                    break;
                case 9:
                    UpdateAbscens(req.AbsID);
                    break;
                case 10:
                    UpdateAbscens(req.AbsID);
                    break;
                default:
                    break;

            }

            //Approve request
            _userRepository.UpdateRequestAnswered(ID, 1);

            //Return to page
            return RedirectToPage("/AdminPage/Fraværes-Anmodninger");
        }

        /// <summary>
        /// Updated abscense of today
        /// </summary>
        /// <param name="id"></param>
        public void UpdateAbscens(int id)
        {
            People = _userRepository.GetPeople();

            Request req = _userRepository.FindRequest(ID);
            PersonalInfo user = People.Find(x => x.ID == req.UserID);
            
            Days day = _userRepository.FindDay(req.Date, user.ID);

            if (day.Days_ID == 0)
            {
                //Variable datetime now
                DateTime now = DateTime.Now;

                //Find date with Startday time and id
                Days toDay = _userRepository.FindDay(new DateTime(now.Year, now.Month, (now.Day - 1), 0, 0, 0), user.ID);

                //Create a Day object
                day = new Days()
                {
                    Date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                    AbscenseID = 1,
                    UserID = user.ID,
                    AbsenceText = "",
                    StartDay = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0),
                    EndDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                    Min = 0,
                    Hour = 0,
                    Saldo = "00:00",
                    TotalSaldo = toDay.TotalSaldo
                };

                _userRepository.CreateDay(day, user.ID);
            }

            if (AbscText == null)
            {
                AbscText = "";
            }

            _userRepository.UpdateAbsence(AbscText, day.Days_ID);
            _userRepository.UpdateAbsencseType(id, day.Days_ID);
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

            //Return to page
            return RedirectToPage("/AdminPage/Fraværes-Anmodninger");
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
            return RedirectToPage("/AdminPage/Fraværes-Anmodninger");
        }
    }
}
