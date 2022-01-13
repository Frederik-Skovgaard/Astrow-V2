﻿using Astrow_2._0.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using Astrow_2._0.Model.Containers;

namespace Astrow_2._0.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITimeCard _timeCard;

        public IndexModel(ITimeCard timeCard)
        {
            _timeCard = timeCard;
        }


        //------------------------Start date | End date------------------------ 
        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }


        //------------------------Input value & current value------------------------
        [BindProperty]
        public string Calendar { get; set; }

       
        [BindProperty]
        public string CalendarValue { get; set; }



        //----------------------List for render days & years----------------------
        [BindProperty]
        public IEnumerable<DateTime> Days { get; set; }


        [BindProperty]
        public IEnumerable<DateTime> Years { get; set; }


        //------------------------User info------------------------
        [BindProperty]
        public LogedUser logged { get; set; }


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
                //To get start & end date of user
                logged = GetDate();

                //Change values
                CalendarValue = DateTime.Now.ToString("MMMM 1, yyyy");

                StartDate = DateTime.Now;

                StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);

                EndDate = StartDate.AddMonths(1);

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));

                Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);


                //Return to page
                return Page();
            }
        }

        /// <summary>
        /// Update days, years & calendar value
        /// </summary>
        public void OnPostLoad()
        {
            //To get start & end date of user
            logged = GetDate();

            //Change values
            StartDate = DateTime.Parse(Calendar);

            if (StartDate < logged.StartDate)
            {
                StartDate = DateTime.Parse($"1-{logged.StartDate.Month}-{logged.StartDate.Year}");

                EndDate = StartDate.AddMonths(1);

                CalendarValue = StartDate.ToString("MMMM dd, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);
            }
            else if (StartDate > logged.EndDate)
            {
                StartDate = DateTime.Parse($"1-{logged.EndDate.Month}-{logged.EndDate.Year}");

                EndDate = StartDate.AddMonths(1);

                CalendarValue = StartDate.ToString("MMMM dd, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);
            }
            else
            {
                EndDate = StartDate.AddMonths(1);

                CalendarValue = Calendar;

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);
            }
        }

        /// <summary>
        /// Method for rendering last month from currnt
        /// </summary>
        public void OnPostBack()
        {
            //To get start & end date of user
            logged = GetDate();

            //Set Start date to input value
            StartDate = DateTime.Parse(Calendar);

            StartDate = StartDate.AddMonths(-1);


            if (StartDate.Year !>= logged.StartDate.Year)
            {
                //Change values
                EndDate = StartDate.AddMonths(1);

                CalendarValue = StartDate.ToString("MMMM 1, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);
            }
            else
            {
                //Change values
                EndDate = StartDate.AddMonths(2);

                CalendarValue = StartDate.AddMonths(1).ToString("MMMM 1, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate.AddMonths(1), EndDate.AddDays(-1));
                Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);

            }
        }

        /// <summary>
        /// Method for rendering current month
        /// </summary>
        public void OnPostNow()
        {
            //To get start & end date of user
            logged = GetDate();

            //Change values
            StartDate = DateTime.Now;

            StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);

            EndDate = StartDate.AddMonths(1);

            CalendarValue = StartDate.ToString("MMMM 1, yyyy");

            //Render days % year/month selector
            Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
            Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);
        }

        /// <summary>
        /// Method for rendering next month from currnt
        /// </summary>
        public void OnPostForward()
        {
            //To get start & end date of user
            logged = GetDate();
             
            //Bug forward when year only

            //Set Start date to input value
            StartDate = DateTime.Parse(Calendar);

            StartDate = StartDate.AddMonths(1);

            //Change 2026 to user enddate & endate.year to date.

            if (StartDate.Year !<= logged.EndDate.Year)
            {
                //Change values
                EndDate = StartDate.AddMonths(1);

                CalendarValue = StartDate.ToString("MMMM 1, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);
            }
            else
            {
                //Change values
                EndDate = StartDate;

                CalendarValue = StartDate.AddMonths(-1).ToString("MMMM 1, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate.AddMonths(-1), EndDate.AddDays(-1));
                Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);
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
    }
}
