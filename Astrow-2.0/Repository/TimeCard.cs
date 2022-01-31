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
        public void CreateDay(Days day, int id)
        {
            stored.CreateDay(day, id);
        }

        /// <summary>
        /// Method for finding all days by date
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<Days> FindAllDays(int id, DateTime date)
        {
            List<Days> days = stored.FindAllDays(id, date);

            return days;
        }

        /// <summary>
        /// Method for finding all days
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<Days> FindAllDaysByID(int id)
        {
            List<Days> days = stored.FindAllDaysByID(id);

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
            stored.UpdateEndDay(day, id);
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
        /// Update total saldo
        /// </summary>
        /// <param name="day"></param>
        /// <param name="id"></param>
        public void UpdateTotalSaldo(Days day, int id)
        {
            stored.UpdateTotalSaldo(day, id);
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
        

        public void Registrer(int id)
        {
            //Variable datetime now
            DateTime now = DateTime.Now;

            //List of all users
            List<Days> daysList = FindAllDays(id, new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));

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
                            UserID = id,
                            StartDay = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0),
                            EndDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                            Saldo = "00:00"
                        };

                        //Add Day object to database 
                        CreateDay(day, id);

                        //Break out of the loop
                        break;
                    }

                    //Eles if today isen't equal to Enday
                    else if (time.EndDay.ToString("HH:mm") == "00:00")
                    {
                        //Find date with Startday time and id
                        Days date = FindDay(time.StartDay, id);

                        //Set EndDay to now
                        date.EndDay = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

                        //Update EndDay in the database
                        UpdateEndDay(date, id);

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
                        UpdateSaldo(date, id);

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
                    UserID = id,
                    StartDay = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0),
                    EndDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0),
                    Saldo = "0"
                };

                //Add Day object to database 
                CreateDay(day, id);
            }
        }
    }
}
