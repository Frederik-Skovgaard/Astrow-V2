using Astrow_2._0.Model.Items;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.DataLayer;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace Astrow_2._0.Repository
{
    public class TimeCard : ITimeCard
    {
        //-------------------Class-------------------------

        StoredProcedure stored = new StoredProcedure();


        //-------------------Methods-----------------------

        /// <summary>
        /// Create Day for the Day table
        /// </summary>
        /// <param name="day"></param>
        /// <param name="user"></param>
        public void CreateDay(Days day, Users user)
        {
            stored.CreateDay(day, user);
        }

        /// <summary>
        /// Update the start of day column in Day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateStartDay(Days day)
        {
            stored.UpdateStartDay(day);
        }

        /// <summary>
        /// Update the end of day column in Day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateEndDay(Days day)
        {
            stored.UpdateEndtDay(day);
        }

        /// <summary>
        /// Update the saldo column in Day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateSaldo(Days day)
        {
            stored.UpdateSaldo(day);
        }

        /// <summary>
        /// Update the absence column in Day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateAbsence(Days day)
        {
            stored.UpdateAbscence(day);
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public IEnumerable<DateTime> EachYear(DateTime from, DateTime thru)
        {
            for (var year = from.Date; year.Year <= thru.Year; year = year.AddYears(1))
                yield return year;
        }
    }
}
