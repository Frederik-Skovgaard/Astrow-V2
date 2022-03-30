﻿using Astrow_2._0.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;

namespace Astrow_2._0.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public IndexModel(IUserRepository timeCard)
        {
            _userRepository = timeCard;
        }


        //------------------------ Start date | End date ------------------------// 
        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }


        //------------------------ Input value & current value ------------------------//
        [BindProperty]
        public string Calendar { get; set; }


        [BindProperty]
        public string CalendarValue { get; set; }


        [BindProperty]
        public List<AbscenseType> AbscenseText { get; set; }


        //------------------------ List for render days & years ------------------------//
        [BindProperty]
        public IEnumerable<DateTime> Days { get; set; }


        [BindProperty]
        public IEnumerable<DateTime> Years { get; set; }


        //------------------------ User info ------------------------//
        [BindProperty]
        public LogedUser logged { get; set; }

        [BindProperty]
        public List<Days> daysList { get; set; }

        [BindProperty]
        public List<AbscenseType> AbscensesRequest { get; set; }


        //------------------------ AbsRequest ------------------------//

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


        //------------------------ Methods ------------------------//

        /// <summary>
        /// On load cheack if logged in. If logged render days & years.
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
                //Get abscense type
                AbscensesRequest = _userRepository.GettAbscenseTypeUserView();


                //To get start & end date of user
                logged = GetDate();
                if (logged != null)
                {
                    //Change values
                    CalendarValue = DateTime.Now.ToString("yyyy-MM");

                    StartDate = DateTime.Now;

                    StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);

                    EndDate = StartDate.AddMonths(1);

                    //Render days % year/month selector
                    Days = _userRepository.EachDay(StartDate, EndDate.AddDays(-1));

                    AbscenseText = _userRepository.GetAbscenseText();

                    //List of all users days
                    daysList = _userRepository.FindAllDaysByID(logged.User_ID);

                    Years = _userRepository.EachYear(logged.StartDate, logged.EndDate);


                    //Return to page
                    return Page();
                }
                else
                {
                    return RedirectToPage("/Login");
                }
            }
        }

        /// <summary>
        /// Update days, years & calendar value
        /// </summary>
        public void OnPostLoad()
        {
            //To get start & end date of user
            logged = GetDate();

            //List of all users days
            daysList = _userRepository.FindAllDaysByID(logged.User_ID);

            AbscenseText = _userRepository.GetAbscenseText();

            //Change values
            StartDate = DateTime.Parse(Calendar);

            if (StartDate <= logged.StartDate)
            {
                StartDate = DateTime.Parse($"{logged.StartDate.Day}-{logged.StartDate.Month}-{logged.StartDate.Year}");

                CalendarValue = StartDate.ToString("yyyy-MM");

                int temp = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);

                EndDate = DateTime.Parse($"{temp}-{StartDate.Month}-{StartDate.Year}");

                //Render days % year/month selector
                Days = _userRepository.EachDay(StartDate, EndDate);
                Years = _userRepository.EachYear(logged.StartDate, logged.EndDate);
            }
            else if (StartDate >= logged.EndDate)
            {
                StartDate = DateTime.Parse($"1-{logged.EndDate.Month}-{logged.EndDate.Year}");

                EndDate = logged.EndDate;

                CalendarValue = StartDate.ToString("yyyy-MM");

                //Render days % year/month selector
                Days = _userRepository.EachDay(StartDate, EndDate);
                Years = _userRepository.EachYear(logged.StartDate, logged.EndDate);
            }
            else
            {
                EndDate = StartDate.AddMonths(1);

                CalendarValue = StartDate.ToString("yyyy-MM");

                //Render days % year/month selector
                Days = _userRepository.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _userRepository.EachYear(logged.StartDate, logged.EndDate);
            }
        }

        /// <summary>
        /// Method for rendering last month from currnt
        /// </summary>
        public void OnPostBack()
        {
            CalendarValue = Request.Form["Calendar"];

            //To get start & end date of user
            logged = GetDate();

            //List of all users days
            daysList = _userRepository.FindAllDaysByID(logged.User_ID);

            AbscenseText = _userRepository.GetAbscenseText();

            try
            {
                //Set Start date to input value
                StartDate = DateTime.Parse(Calendar);
            }
            catch (Exception)
            {
                Calendar = DateTime.Now.AddMonths(1).ToString("yyyy-MM");
            }
            finally
            {
                StartDate = DateTime.Parse(Calendar);
            }

            StartDate = StartDate.AddMonths(-1);


            if (StartDate <= logged.StartDate)
            {
                StartDate = DateTime.Parse($"{logged.StartDate.Day}-{logged.StartDate.Month}-{logged.StartDate.Year}");

                CalendarValue = StartDate.ToString("yyyy-MM");

                int temp = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);

                EndDate = DateTime.Parse($"{temp}-{StartDate.Month}-{StartDate.Year}");

                //Render days % year/month selector
                Days = _userRepository.EachDay(StartDate, EndDate);
                Years = _userRepository.EachYear(logged.StartDate, logged.EndDate);
            }
            else
            {
                EndDate = StartDate.AddMonths(1);

                CalendarValue = StartDate.ToString("yyyy-MM");

                //Render days % year/month selector
                Days = _userRepository.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _userRepository.EachYear(logged.StartDate, logged.EndDate);
            }
        }

        /// <summary>
        /// Method for rendering current month
        /// </summary>
        public IActionResult OnPostNow()
        {
            //Return to home page
            return RedirectToPage("/Home");
        }

        /// <summary>
        /// Method for rendering next month from currnt
        /// </summary>
        public void OnPostForward()
        {
            CalendarValue = Request.Form["Calendar"];

            //To get start & end date of user
            logged = GetDate();

            //List of all users days
            daysList = _userRepository.FindAllDaysByID(logged.User_ID);

            AbscenseText = _userRepository.GetAbscenseText();

            //Bug forward when year only

            try
            {
                //Set Start date to input value
                StartDate = DateTime.Parse(Calendar);
            }
            catch (Exception)
            {
                Calendar = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");
                
            }
            finally
            {
                StartDate = DateTime.Parse(Calendar);
            }

            StartDate = StartDate.AddMonths(1);

            //Change 2026 to user enddate & endate.year to date.

            if (StartDate >= logged.EndDate)
            {
                StartDate = DateTime.Parse($"1-{logged.EndDate.Month}-{logged.EndDate.Year}");

                EndDate = logged.EndDate;

                CalendarValue = StartDate.ToString("yyyy-MM");

                //Render days % year/month selector
                Days = _userRepository.EachDay(StartDate, EndDate);
                Years = _userRepository.EachYear(logged.StartDate, logged.EndDate);
            }
            else
            {
                EndDate = StartDate.AddMonths(1);

                CalendarValue = StartDate.ToString("yyyy-MM");

                //Render days % year/month selector
                Days = _userRepository.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _userRepository.EachYear(logged.StartDate, logged.EndDate);
            }
        }


        //------------------------ Nav Methods ------------------------//

        /// <summary>
        /// Method for clocking in and out
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostRegistrering()
        {

            if (HttpContext.Session.GetInt32("_UserID") != null)
            {
                //User ID
                int id = (int)HttpContext.Session.GetInt32("_UserID");

                //Method for registry
                _userRepository.Registrer(id);

                //Return to home page
                return RedirectToPage("/Home");
            }
            else
            {
                //Return to home page
                return RedirectToPage("/Home");
            }
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
            return RedirectToPage("/Home");
        }
    }
}
