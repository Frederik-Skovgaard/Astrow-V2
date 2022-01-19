using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Repository;
using Microsoft.AspNetCore.Http;

namespace Astrow_2._0.Pages.Shared
{
    public class _Layout : PageModel
    {
        private readonly ITimeCard _timeCard;

        public _Layout(ITimeCard timeCard)
        {
            _timeCard = timeCard;
        }


        public void OnGet()
        {

        }

        public IActionResult OnPostRegistrering()
        {
            string id = HttpContext.Session.GetString("_UserID");

            DateTime da = DateTime.Now;

            DateTime ad = new DateTime(da.Year, da.Month, da.Day, da.Hour, da.Minute, 0);

            Days day = new Days()
            {
                Date = DateTime.Now,
                User_ID = Convert.ToInt32(id),
                StartDay = ad
            };

            _timeCard.CreateDay(day);

            return RedirectToPage("/HomePage");
        }
    }
}
