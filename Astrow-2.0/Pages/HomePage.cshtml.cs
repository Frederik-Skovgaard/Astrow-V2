using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Astrow_2._0.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace Astrow_2._0.Pages
{
    
    public class IndexModel : PageModel
    {
        private readonly ITimeCard _timeCard;

        public IndexModel(ITimeCard timeCard)
        {
            _timeCard = timeCard;
        }

        //Start date 
        [BindProperty]
        public DateTime StartDate { get; set; }

        //One month after start date
        [BindProperty]
        public DateTime EndDate { get; set; }


        //Input filed value
        [BindProperty]
        public string Calendar { get; set; }

        //Value of input filed
        [BindProperty]
        public string CalendarValue { get; set; }



        //List with all the days between start & end date
        [BindProperty]
        public IEnumerable<DateTime> Days { get; set; }

        //List with all the years between start & end date
        [BindProperty]
        public IEnumerable<DateTime> Years { get; set; }

        

        




        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("_UserID") == 0)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                //Change values
                CalendarValue = DateTime.Now.ToString("MMMM 1, yyyy");

                StartDate = DateTime.Now;
                EndDate = DateTime.Now.AddMonths(1);

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _timeCard.EachYear(StartDate, DateTime.Now.AddYears(4));


                //Return to page
                return Page();
            }
        }

        /// <summary>
        /// Update days, years & calendar value
        /// </summary>
        public void OnPostLoad()
        {
            //Change values
            StartDate = DateTime.Parse(Calendar);
            EndDate = StartDate.AddMonths(1);

            CalendarValue = Calendar;

            //Render days % year/month selector
            Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
            Years = _timeCard.EachYear(DateTime.Now, DateTime.Now.AddYears(4));
        }

        /// <summary>
        /// Go one month back
        /// </summary>
        public void OnPostBack()
        {
            
            StartDate = DateTime.Parse(Calendar);

            StartDate = StartDate.AddMonths(-1);

            //Change 2020 to user startdate & startDate.year to date.
            if (StartDate.Year != 2020)
            {
                //Change values
                EndDate = StartDate.AddMonths(1);

                CalendarValue = StartDate.ToString("MMMM 1, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _timeCard.EachYear(DateTime.Now, DateTime.Now.AddYears(4));
            }
            else
            {
                //Change values
                EndDate = StartDate.AddMonths(2);

                CalendarValue = StartDate.AddMonths(1).ToString("MMMM 1, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate.AddMonths(1), EndDate.AddDays(-1));
                Years = _timeCard.EachYear(DateTime.Now, DateTime.Now.AddYears(4));
            }
        }

        public void OnPostNow()
        {
            //Change values
            StartDate = DateTime.Now;
            EndDate = StartDate.AddMonths(1);

            CalendarValue = StartDate.ToString("MMMM 1, yyyy");

            //Render days % year/month selector
            Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
            Years = _timeCard.EachYear(DateTime.Now, DateTime.Now.AddYears(4));
        }

        public void OnPostForward()
        {
            StartDate = DateTime.Parse(Calendar);

            StartDate = StartDate.AddMonths(1);

            //Change 2026 to user enddate & endate.year to date.
            
            if (StartDate.Year != 2026)
            {
                //Change values
                EndDate = StartDate.AddMonths(1);

                CalendarValue = StartDate.ToString("MMMM 1, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate, EndDate.AddDays(-1));
                Years = _timeCard.EachYear(DateTime.Now, DateTime.Now.AddYears(4));
            }
            else
            {
                //Change values
                EndDate = StartDate;

             
                CalendarValue = StartDate.AddMonths(-1).ToString("MMMM 1, yyyy");

                //Render days % year/month selector
                Days = _timeCard.EachDay(StartDate.AddMonths(-1), EndDate.AddDays(-1));
                Years = _timeCard.EachYear(DateTime.Now, DateTime.Now.AddYears(4));
            }
        }
    }
}
