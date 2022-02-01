using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.DataLayer;
using Astrow_2._0.CustomExceptions;
using System.Security.Cryptography;
using System.Text;

namespace Astrow_2._0.Repository
{
    public class UserRepository : IUserRepository
    {

        //-------------------Class-------------------------

        StoredProcedure stored = new StoredProcedure();


        //-------------------Methods-----------------------


        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(Users user, UserPersonalInfo info)
        {
            stored.CreateUsers(user, info);
        }

        /// <summary>
        /// Delete user from database
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(int id)
        {
            stored.DeleteUser(id);
        }

        /// <summary>
        /// Update user parameters
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(Users user)
        {
            stored.UpdateUser(user);
        }

        /// <summary>
        /// Update user's infomation
        /// </summary>
        /// <param name="person"></param>
        public void UpdateUserInfo(UserPersonalInfo person)
        {
            stored.UpdateUserInfo(person);
        }

        /// <summary>
        /// Return all users to list
        /// </summary>
        /// <returns></returns>
        public List<Users> ReadAllUsers()
        {
            List<Users> userList = stored.ReadAllUsers();

            return userList;
        }

        /// <summary>
        /// Find User by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Users FindUser(int id)
        {
            Users user = stored.FindByID(id);
            return user;
        }

        /// <summary>
        /// Find salt by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Users FindByUserName(string username)
        {
            Users user = stored.FindByUserName(username);
            return user;
        }

        public UserPersonalInfo FindUserInfo(int id)
        {
            UserPersonalInfo person = stored.FindUserInfo(id);

            return person;
        }

        /// <summary>
        /// Method for loggin in
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LogedUser Login(string username, string password)
        {
            List<Users> userList = ReadAllUsers();

            LogedUser logedUser = new LogedUser();

            Users user = userList.SingleOrDefault(x => x.UserName == username && x.Password == password);

            if (user != null)
            {
                return logedUser = new LogedUser
                {
                    UserName = user.UserName,
                    Status = user.Status,
                    User_ID = user.User_ID,
                    StartDate = user.StartDate,
                    EndDate = user.EndDate
                };
            }
            else
            {
                return logedUser;
            }
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="id"></param>
        public void Registrer(int id)
        {
            TimeCard timeCard = new TimeCard();

            //Variable datetime now
            DateTime now = DateTime.Now;

            //List of all users
            List<Days> daysList = timeCard.FindAllDays(id, new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));

            //Get totalsaldo of last day
            Days totalSaldo = timeCard.FindTotalSaldo();

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
                        timeCard.CreateDay(day, id);

                        //Break out of the loop
                        break;
                    }

                    //Eles if today isen't equal to Enday
                    else if (time.EndDay.ToString("HH:mm") == "00:00")
                    {
                        //Find date with Startday time and id
                        Days date = timeCard.FindDay(time.StartDay, id);

                        //Get totalsaldo of last day
                        Days dayBefore = timeCard.FindDayByID(date.Days_ID - 1);

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
                        timeCard.UpdateDay(date);

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
                    Min = 0,
                    Hour = 0,
                    Saldo = "00:00",
                    TotalSaldo = totalSaldo.TotalSaldo
                };

                //Add Day object to database 
                timeCard.CreateDay(day, id);
            }
        }

        #region Encryption

        public string CreateSalt(int size)
        {
            RNGCryptoServiceProvider rngC = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rngC.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }
        public string GenerateHash(string password, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public bool PasswordAreEqual(string plainTextPassword, string hashedPassword, string salt)
        {
            string newHashedPin = GenerateHash(plainTextPassword, salt);
            return newHashedPin.Equals(hashedPassword);
        }
        #endregion
    }
}
