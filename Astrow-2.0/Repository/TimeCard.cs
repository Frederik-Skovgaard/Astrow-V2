﻿using Astrow_2._0.Model.Items;
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

        /// <summary>
        /// Find day from id
        /// </summary>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Days FindDay(DateTime date, int id)
        {
            Days day = stored.FindDay(date, id);
            return day;
        }

        /// <summary>
        /// Method for finding day by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Days FindTotalSaldo()
        {
            Days day = stored.FindTotalSaldo();
            return day;
        }

        /// <summary>
        /// Find day by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Days FindDayByID(int id)
        {
            Days day = stored.FindDayByID(id);
            return day;
        }

        /// <summary>
        /// Update day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateDay(Days day)
        {
            stored.UpdateDay(day);
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
        
        /// <summary>
        /// Sign user in or out
        /// </summary>
        /// <param name="id"></param>
        public void Registrer(int id)
        {
            //Variable datetime now
            DateTime now = DateTime.Now;

            //List of all users
            List<Days> daysList = FindAllDays(id, new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));

            //Get totalsaldo of last day
            Days totalSaldo = FindTotalSaldo();

            //If there was none
            if (totalSaldo.Days_ID == 0)
            {
                totalSaldo.Min = 0;
                totalSaldo.Hour = 0;
                totalSaldo.TotalSaldo = "00:00";
            }


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
                            Min = 0,
                            Hour = 0,
                            Saldo = "00:00",
                            TotalSaldo = totalSaldo.TotalSaldo
                        };

                        //Add Day object to database 
                        CreateDay(day, id);

                        break;
                    }

                    //Eles if today isen't equal to Enday
                    else if (time.EndDay.ToString("HH:mm") == "00:00")
                    {
                        //Find date with Startday time and id
                        Days date = FindDay(time.StartDay, id);

                        //Get totalsaldo of last day
                        Days dayBefore = FindDayByID(date.Days_ID - 1);

                        //If there was none
                        if (dayBefore.Days_ID == 0)
                        {
                            dayBefore.Min = 0;
                            dayBefore.Hour = 0;
                            dayBefore.TotalSaldo = "00:00";
                        }

                        //Set EndDay to now
                        date.EndDay = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

                        DateTime StartOfDay = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
                        DateTime EndOfDay = new DateTime(now.Year, now.Month, now.Day, 15, 24, 0);


                        //-7:24
                        TimeSpan ts = StartOfDay - EndOfDay;

                        //Start day minus end day 
                        TimeSpan tempSpan = date.EndDay - date.StartDay;

                        //Time between start and end date plus -7:24 
                        TimeSpan saldo = tempSpan + ts;


                        //Remove the minus from the minut and hour
                        int min = 0;
                        int hour = 0;

                        //If minut is negative remove minus
                        if (saldo.Minutes < 0)
                        {
                            min = saldo.Minutes * -1;
                        }
                        else
                        {
                            min = saldo.Minutes;
                        }

                        //If hour is negative remove minus
                        if (saldo.Hours < 0)
                        {
                            hour = saldo.Hours * -1;
                        }
                        else
                        {
                            hour = saldo.Hours;
                        }

                        //Temp string holder
                        string tempMin = "";
                        string tempHour = "";

                        //If minut lenght equals to one add 0 infront  
                        if (min.ToString().Length == 1)
                        {
                            tempMin = $"0{min}";
                        }
                        else
                        {
                            tempMin = min.ToString();
                        }

                        //If hours lenght equals to one add 0 infront  
                        if (hour.ToString().Length == 1)
                        {
                            //If hour is negativ add minus infront
                            if (saldo.Hours < 0)
                            {
                                tempHour = $"-0{hour}";
                            }
                            else
                            {
                                tempHour = hour.ToString();
                            }
                            
                        }

                        //Set min and hour
                        date.Min = min;
                        date.Hour = hour;

                        //Set Saldo to saldo value
                        date.Saldo = $"{tempHour}:{tempMin}";

                        //If total saldo minut is negative remove the minus
                        int minut = saldo.Minutes + dayBefore.Min;

                        //string holder for new total saldo
                        string tempMinStr = "";

                        //If number negative 
                        if (minut < 0)
                        {
                            minut = minut * -1;
                        }

                        //If number is one digit long add zero infront
                        if (minut.ToString().Length == 1)
                        {
                            //If number is negative add minus infront
                            if (saldo.Hours < 0)
                            {
                                tempMinStr = $"0{minut}";
                            }
                            else
                            {
                                tempMinStr = minut.ToString();
                            }
                        }

                        //Old total saldo plus new saldo
                        int hourTemp = saldo.Hours + dayBefore.Hour;

                        //string holder for new total saldo
                        string tempTotStr = "";

                        //If number negative 
                        if (hourTemp < 0)
                        {
                            hourTemp = hourTemp * -1;
                        }


                        //If number is one digit long add zero infront
                        if (hourTemp.ToString().Length == 1)
                        {
                            //If number is negative add minus infront
                            if (saldo.Hours < 0)
                            {
                                tempTotStr = $"-0{hourTemp}";
                            }
                            else
                            {
                                tempTotStr = hourTemp.ToString();
                            }

                        }


                        //Set total saldo
                        date.TotalSaldo = $"{tempTotStr}:{tempMinStr}";


                        //Update day
                        UpdateDay(date);

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
                    Min = 0,
                    Hour = 0,
                    Saldo = "00:00",
                    TotalSaldo = totalSaldo.TotalSaldo
                };

                //Add Day object to database 
                CreateDay(day, id);  
            }            
        }
    }
}
