using Astrow_2._0.Repository;
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

        [BindProperty]
        public List<Days> daysList { get; set; }


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
                if (logged != null)
                {
                    
                    //Change values
                    CalendarValue = DateTime.Now.ToString("MMMM 1, yyyy");

                    StartDate = DateTime.Now;

                    StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);

                    EndDate = StartDate.AddMonths(1);

                    //Render days % year/month selector
                    Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));

                    //List of all users days
                    daysList = _timeCard.FindAllDays(logged.User_ID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));

                    Years = _timeCard.EachYear(logged.StartDate, logged.EndDate);


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

            //List of all users days
            daysList = _timeCard.FindAllDays(logged.User_ID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));

            try
            {
                //Set Start date to input value
                StartDate = DateTime.Parse(Calendar);
            }
            catch (Exception)
            {
                Calendar = DateTime.Now.AddMonths(1).ToString("MMMM 1, yyyy");
            }
            finally
            {
                StartDate = DateTime.Parse(Calendar);
            }

            StartDate = StartDate.AddMonths(-1);


            if (StartDate.Year! >= logged.StartDate.Year)
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
        public IActionResult OnPostNow()
        {
            //Return to home page
            return RedirectToPage("/HomePage");
        }

        /// <summary>
        /// Method for rendering next month from currnt
        /// </summary>
        public void OnPostForward()
        {
            //To get start & end date of user
            logged = GetDate();

            //List of all users days
            daysList = _timeCard.FindAllDays(logged.User_ID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));

            //Bug forward when year only

            try
            {
                //Set Start date to input value
                StartDate = DateTime.Parse(Calendar);
            }
            catch (Exception)
            {
                Calendar = DateTime.Now.AddMonths(-1).ToString("MMMM 1, yyyy");
                
            }
            finally
            {
                StartDate = DateTime.Parse(Calendar);
            }

            StartDate = StartDate.AddMonths(1);

            //Change 2026 to user enddate & endate.year to date.

            if (StartDate.Year! <= logged.EndDate.Year)
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
        /// Method for clocking in and out
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostRegistrering()
        {
            //Variable datetime now
            DateTime now = DateTime.Now;

            //User ID
            int id = (int)HttpContext.Session.GetInt32("_UserID");

            //List of all users
            List<Days> daysList = _timeCard.FindAllDays(id, new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));

            //If list is empty
            if (daysList.Count != 0)
            {

                //Find days with datetime now and users id
                foreach (Days time in daysList)
                {

                    //If today isen't the same day
                    if (DateTime.Now.ToString("yyyy/MM/dd") != time.Date.ToString("yyyy/MM/dd"))
                    {
                        //Create a Day object
                        Days day = new Days()
                        {
                            Date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                            User_ID = id,
                            StartDay = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0),
                            EndDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                            Saldo = "0"
                        };

                        //Add Day object to database 
                        _timeCard.CreateDay(day);

                        //Break out of the loop
                        break;
                    }

                    //Eles if today isen't equal to Enday
                    else if (time.EndDay.ToString("HH:mm") == "00:00")
                    {
                        //Find date with Startday time and id
                        Days date = _timeCard.FindDay(time.StartDay, id);

                        //Set EndDay to now
                        date.EndDay = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

                        //Update EndDay in the database
                        _timeCard.UpdateEndDay(date, id);

                        DateTime StartOfDay = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
                        DateTime EndOfDay = new DateTime(now.Year, now.Month, now.Day, 15, 24, 0);


                        //-7:24
                        TimeSpan ts = StartOfDay - EndOfDay;

                        //Start day minus end day 
                        TimeSpan tempSpan = date.EndDay - date.StartDay;

                        //Time between start and end date plus -7:24 
                        TimeSpan saldo = tempSpan + ts;


                        int min = 0;
                        if (saldo.Minutes < 0)
                        {
                            min = saldo.Minutes * -1;
                        }
                        else
                        {
                            min = saldo.Minutes;
                        }

                        //Set Saldo to saldo value
                        date.Saldo = $"{saldo.Hours}:{min}";

                        //Update Saldo in the database
                        _timeCard.UpdateSaldo(date, id);

                        //Break out of the loop
                        break;
                    }
                }
            }
            else
            {
                //Create a Day object
                Days day = new Days()
                {
                    Date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                    User_ID = id,
                    StartDay = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0),
                    EndDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                    Saldo = "0"
                };

                //Add Day object to database 
                _timeCard.CreateDay(day);
            }

            //Return to home page
            return RedirectToPage("/HomePage");
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
