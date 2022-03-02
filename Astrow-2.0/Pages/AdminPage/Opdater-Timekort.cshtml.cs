using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Astrow_2._0.Model.Items;
using System.Linq;

namespace Astrow_2._0.Pages.AdminPage
{
    public class UpdateTimeCardModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public UpdateTimeCardModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [BindProperty]
        public bool DateBool { get; set; }

        [BindProperty]
        public bool TimeBool { get; set; }

        [BindProperty]
        public List<Users> UserList { get; set; }


        [BindProperty]
        public bool Abscense { get; set; }

        [BindProperty]
        public string AbscenseText { get; set; }


        [BindProperty]
        public string StartDay { get; set; }

        [BindProperty]
        public string EndDay { get; set; }


        [BindProperty]
        public string StartHour { get; set; }

        [BindProperty]
        public string EndHour { get; set; }


        [BindProperty]
        public string AbsenceType { get; set; }

        [BindProperty]
        public string CalenderValue { get; set; }

        [BindProperty]
        public List<AbscenseType> AbscenseType { get; set; }

        [BindProperty]
        public int ID { get; set; }


        /// <summary>
        /// Check if user is logged and has premission
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
                if (HttpContext.Session.GetString("_Status") != "Instructør")
                {
                    return RedirectToPage("/Home");
                }
                else
                {
                    StartDay = $"{DateTime.Now.Year}-{DateTime.Now.Month}";
                    EndDay = $"{DateTime.Now.Year}-{DateTime.Now.Month}";

                    //Fills dropdown with users 
                    UserList = _userRepository.ReadAllUsers();

                    //Set calender value to today
                    CalenderValue = DateTime.Now.ToString("yyyy-MM-dd");

                    DateBool = false;

                    TimeBool = false;

                    return Page();
                }
            }

        }

        /// <summary>
        /// Get user by id
        /// </summary>
        public IActionResult OnPostUser()
        {
            if (ID != 0)
            {
                List<Days> days = _userRepository.FindAllDaysByID(ID);

                if (days.Count != 0)
                {
                    Days firstElement = days.First();
                    Days lastElement = days.Last();

                    StartDay = firstElement.Date.ToString("yyyy-MM-dd");
                    EndDay = lastElement.Date.ToString("yyyy-MM-dd");

                    DateBool = true;

                    TimeBool = false;

                    //Fills dropdown with users
                    UserList = _userRepository.ReadAllUsers();
                }
                else
                {
                    return RedirectToPage("/AdminPage/Opdater-Timekort");
                }
            }
            else
            {
                //Fills dropdown with users
                UserList = _userRepository.ReadAllUsers();
            }

            return Page();
        }

        /// <summary>
        /// Fill out the fields with the infomation of the selected date
        /// </summary>
        public void OnPostLoad()
        {
            //Get abscense type
            AbscenseType = _userRepository.GetAllAbscenseType();

            if (ID != 0)
            {
                CalenderValue = Request.Form["Calendar"];

                DateTime calenderValue = DateTime.Parse(CalenderValue);

                Users user = _userRepository.FindUser(ID);
                Days day = _userRepository.FindDay(calenderValue, ID);

                List<Days> days = _userRepository.FindAllDaysByID(ID);

                Days firstElement = days.First();
                Days lastElement = days.Last();

                StartDay = firstElement.Date.ToString("yyyy-MM-dd");
                EndDay = lastElement.Date.ToString("yyyy-MM-dd");

                StartHour = day.StartDay.ToString("HH:mm");
                EndHour = day.EndDay.ToString("HH:mm");


                AbsenceType = day.AbscenseID.ToString();

                DateBool = true;

                TimeBool = true;

                //Fills dropdown with users
                UserList = _userRepository.ReadAllUsers();
            }
            else
            {
                //Fills dropdown with users
                UserList = _userRepository.ReadAllUsers();
            }
        }

        public IActionResult OnPostSubmit()
        {
            //Date picker value
            CalenderValue = Request.Form["Calendar"];

            Days day = _userRepository.FindDay(DateTime.Parse(CalenderValue), ID);

            Days dayBefore = _userRepository.FindDayByID((day.Days_ID - 1));

            List<Days> daysList = _userRepository.FindAllDaysByID(ID);


            
            DateTime calenderValue = DateTime.Parse(CalenderValue);

            //Set Start and End of day to datetime
            DateTime startHour = DateTime.Parse(StartHour);
            DateTime startHourDT = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day, startHour.Hour, startHour.Minute, 0);

            DateTime endHour = DateTime.Parse(EndHour);
            DateTime endHourDT = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day, endHour.Hour, endHour.Minute, 0);


            if (ID != 0)
            {

                //Update Start and End of day
                _userRepository.UpdateStartDay(startHourDT, day.Days_ID);
                _userRepository.UpdateEndDay(endHourDT, day.Days_ID);


                //-7:24
                TimeSpan workHours = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0) - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 24, 0);

                //Start day minus end day 
                TimeSpan workTime = endHourDT - startHourDT;

                //Time between start and end date plus -7:24 
                TimeSpan saldo = workTime + workHours;

                if (day.StartDay != startHourDT || day.EndDay != endHourDT)
                {
                    //New total saldo
                    if (dayBefore.TotalHour <= 0 && saldo.Hours <= 0 || dayBefore.TotalHour >= 0 && saldo.Hours >= 0)
                    {
                        dayBefore.TotalHour = dayBefore.TotalHour + saldo.Hours;
                    }
                    else if (dayBefore.TotalHour < 0 && saldo.Hours > 0 || dayBefore.TotalHour > 0 && saldo.Hours < 0)
                    {
                        dayBefore.TotalHour = saldo.Hours - dayBefore.TotalHour;
                    }

                    if (dayBefore.TotalMin <= 0 && saldo.Minutes <= 0)
                    {
                        dayBefore.TotalMin = dayBefore.TotalMin + saldo.Minutes;
                    }
                    else if (dayBefore.TotalMin < 0 && saldo.Minutes > 0 || dayBefore.TotalMin > 0 && saldo.Minutes < 0)
                    {
                        dayBefore.TotalMin = saldo.Minutes + dayBefore.TotalMin;
                    }

                    //If minut reach 60 min
                    if (dayBefore.TotalMin >= 60 || dayBefore.TotalMin <= -60)
                    {
                        //If hour is negative
                        if (dayBefore.TotalMin < 0)
                        {
                            dayBefore.TotalMin = dayBefore.TotalMin + 60;
                            dayBefore.TotalHour = dayBefore.TotalHour - 1;
                        }
                        else
                        {
                            dayBefore.TotalMin = dayBefore.TotalMin - 60;
                            dayBefore.TotalHour = dayBefore.TotalHour + 1;
                        }
                    }
                }

                string hour = "";
                string minut = "";

                //If minut lenght equals to one add 0 infront  
                if (saldo.Minutes < 0)
                {
                    if ((saldo.Minutes * -1).ToString().Length == 1)
                    {
                        minut = $"0{(saldo.Minutes * -1)}";
                    }
                    else
                    {
                        minut = (saldo.Minutes * -1).ToString();
                    }
                }
                else
                {
                    if (saldo.Minutes.ToString().Length == 1)
                    {
                        minut = $"0{saldo.Minutes}";
                    }
                    else
                    {
                        minut = saldo.Minutes.ToString();
                    }
                }

                //If hours lenght equals to one add 0 infront  
                if (saldo.Hours < 0)
                {
                    if ((saldo.Hours * -1).ToString().Length == 1)
                    {
                        hour = $"-0{(saldo.Hours * -1)}";
                    }
                    else
                    {
                        hour = $"0{saldo.Hours * -1}".ToString();
                    }
                }
                else
                {
                    if (saldo.Hours.ToString().Length == 1)
                    {
                        hour = $"0{saldo.Hours}".ToString();
                    }
                    else
                    {
                        hour = $"0{saldo.Hours}".ToString();
                    }
                }



                string totalHour = "";
                string totalMinut = "";

                //If minut lenght equals to one add 0 infront  
                if (dayBefore.TotalMin < 0)
                {
                    if ((dayBefore.TotalMin * -1).ToString().Length == 1)
                    {
                        totalMinut = $"0{(dayBefore.TotalMin * -1)}";
                    }
                    else
                    {
                        totalMinut = (dayBefore.TotalMin * -1).ToString();
                    }
                }
                else
                {
                    if (dayBefore.TotalMin.ToString().Length == 1)
                    {
                        totalMinut = $"0{dayBefore.TotalMin}";
                    }
                    else
                    {
                        totalMinut = dayBefore.TotalMin.ToString();
                    }
                }

                //If hours lenght equals to one add 0 infront  
                if (dayBefore.TotalHour < 0)
                {
                    if ((dayBefore.TotalHour * -1).ToString().Length == 1)
                    {
                        totalHour = $"-0{(dayBefore.TotalHour * -1)}";
                    }
                    else
                    {
                        totalHour = (dayBefore.TotalHour * -1).ToString();
                    }
                }
                else
                {
                    if (dayBefore.TotalHour.ToString().Length == 1)
                    {
                        totalHour = $"0{dayBefore.TotalHour}";
                    }
                    else
                    {
                        totalHour = dayBefore.TotalHour.ToString();
                    }
                }



                //Total saldo
                string totalSaldoStr = $"{totalHour}:{totalMinut}";

                string saldoStr = $"{hour}:{minut}";


                //Update saldo and total saldo
                _userRepository.UpdateSaldo(saldo.Minutes, saldo.Hours, saldoStr, day.Days_ID);
                _userRepository.UpdateTotalSaldo(dayBefore.TotalMin, dayBefore.TotalHour, totalSaldoStr, day.Days_ID);


                Days tmp = new Days();

                //Foreach day 
                foreach (Days days in daysList)
                {
                    
                    if (days.Days_ID != 0)
                    {
                        tmp = _userRepository.FindDayByID((days.Days_ID - 1));
                    }
                    

                    if (days.Date > day.Date)
                    {
                        //New total saldo
                        if (dayBefore.TotalHour <= 0)
                        {
                            days.TotalHour = days.Hour + tmp.TotalHour;
                        }
                        else if (dayBefore.TotalHour > 0)
                        {
                            days.TotalHour = tmp.TotalHour - days.Hour;
                        }

                        if (dayBefore.TotalMin <= 0)
                        {
                            days.TotalMin = days.Min + tmp.TotalMin;
                        }
                        else if (dayBefore.TotalMin > 0)
                        {
                            days.TotalMin = days.Min + tmp.TotalMin;
                        }

                        //If hour > 0 && min < 0
                        if (days.TotalHour > 0 && days.TotalMin < 0)
                        {
                            days.TotalMin = 60 + days.TotalMin;
                            days.TotalHour = days.TotalHour - 1;
                        }


                        //If minut reach 60 min
                        if (days.TotalMin >= 60 || days.TotalMin <= -60)
                        {
                            //If hour is negative
                            if (days.TotalMin < 0)
                            {
                                days.TotalMin = days.TotalMin + 60;
                                days.TotalHour = days.TotalHour - 1;
                            }
                            else
                            {
                                days.TotalMin = days.TotalMin - 60;
                                days.TotalHour = days.TotalHour + 1;
                            }
                        }

                        string totalHourLoop = "";
                        string totalMinutLoop = "";

                        //If minut lenght equals to one add 0 infront  
                        if (days.TotalMin < 0)
                        {
                            if ((days.TotalMin * -1).ToString().Length == 1)
                            {
                                totalMinutLoop = $"0{(days.TotalMin * -1)}";
                            }
                            else
                            {
                                totalMinutLoop = (days.TotalMin * -1).ToString();
                            }
                        }
                        else
                        {
                            if (days.TotalMin.ToString().Length == 1)
                            {
                                totalMinutLoop = $"0{days.TotalMin}";
                            }
                            else
                            {
                                totalMinutLoop = days.TotalMin.ToString();
                            }
                        }

                        //If hours lenght equals to one add 0 infront  
                        if (days.TotalHour < 0)
                        {
                            if ((days.TotalHour * -1).ToString().Length == 1)
                            {
                                totalHourLoop = $"-0{(days.TotalHour * -1)}";
                            }
                            else
                            {
                                totalHourLoop = (days.TotalHour * -1).ToString();
                            }
                        }
                        else
                        {
                            if (days.TotalHour.ToString().Length == 1)
                            {
                                totalHourLoop = $"0{days.TotalHour}";
                            }
                            else
                            {
                                totalHourLoop = days.TotalHour.ToString();
                            }
                        }

                        //Total saldo
                        string updateSaldo = $"{totalHourLoop}:{totalMinutLoop}";
                        _userRepository.UpdateTotalSaldo(days.TotalMin, days.TotalHour, updateSaldo, days.Days_ID);
                    }
                }

                if (AbscenseText != null && AbsenceType != "1")
                {
                    _userRepository.UpdateAbsence(AbscenseText, day.Days_ID);
                    _userRepository.UpdateAbsencseType(Convert.ToInt32(AbsenceType), day.Days_ID);
                }

                if (AbscenseText != null)
                {
                    _userRepository.UpdateAbsence(AbscenseText, day.Days_ID);
                }
                if (AbsenceType != "1")
                {
                    _userRepository.UpdateAbsencseType(Convert.ToInt32(AbsenceType), day.Days_ID);
                }


                return RedirectToPage("/AdminPage/Opdater-Timekort");
            }
            else
            {
                return RedirectToPage("/AdminPage/Opdater-Timekort");
            }

        }

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

            //Return to home page
            return RedirectToPage("/AdminPage/Opdater-Timekort");
        }
    }
}
