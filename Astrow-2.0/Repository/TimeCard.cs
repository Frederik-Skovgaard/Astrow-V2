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
        public void CreateDay(Days day)
        {
            stored.CreateDay(day);
        }

        /// <summary>
        /// Method for finding all days
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<Days> FindAllDays(int id, DateTime date)
        {
            List<Days> days = stored.FindAllDays(id, date);

            return days;
        }

        public Days FindDay(DateTime date, int id)
        {
            Days day = stored.FindDay(date, id);
            return day;
        }

        /// <summary>
        /// Update the start of day column in Day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateStartDay(Days day, int id)
        {
            stored.UpdateStartDay(day, id);
        }

        /// <summary>
        /// Update the end of day column in Day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateEndDay(Days day, int id)
        {
            stored.UpdateEndtDay(day, id);
        }

        /// <summary>
        /// Update the saldo column in Day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateSaldo(Days day, int id)
        {
            stored.UpdateSaldo(day, id);
        }

        /// <summary>
        /// Update the absence column in Day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateAbsence(Absence abs, Days day)
        {
            stored.UpdateAbscence(abs, day);
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public IEnumerable<DateTime> EachYear(DateTime from, DateTime thru)
        {
            for (var year = from.Date; year.Year <= thru.Year; year = year.AddYears(1))
            {
                yield return year;
            }
                
        }
        
    }
}
