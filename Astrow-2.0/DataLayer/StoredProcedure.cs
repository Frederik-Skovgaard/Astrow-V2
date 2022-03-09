using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Model.Containers;

namespace Astrow_2._0.DataLayer
{

    public class StoredProcedure
    {
        //SQL connection
        #region SQL
        internal static IConfigurationRoot configuration { get; set; }
        static string connectionString;

        SqlConnection sql;

        /// <summary>
        /// SQL Connection
        /// </summary>
        public static void SetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            configuration = builder.Build();
            connectionString = configuration.GetConnectionString("AstrowDatabase");
        }
        #endregion

        
        #region Create


        /// <summary>
        /// Creates 
        /// </summary>
        /// <param name="user"></param>
        public void CreateUsers(Users user, UserPersonalInfo info)
        {
            info = CreateUserInfo(info);
            CreateUser(user, info);
        }

        //--------- Container ---------

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user"></param>
        public Users CreateUser(Users user, UserPersonalInfo info)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("CreateUser", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", info.Name_ID);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Status", user.Status);
                cmd.Parameters.AddWithValue("@salt", user.Salt);
                cmd.Parameters.AddWithValue("@startDate", user.StartDate);
                cmd.Parameters.AddWithValue("@endDate", user.EndDate);

                cmd.ExecuteNonQuery();
            }

            //Find ID
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("GetUser", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@userName", user.UserName);
                cmd.Parameters.AddWithValue("@password", user.Password);

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        user = new Users
                        {
                            User_ID = read.GetInt32(0),
                            UserName = read.GetString(1),
                            Password = read.GetString(2),
                            Name_ID = read.GetInt32(3),
                            Status = read.GetString(4),
                            StartDate = read.GetDateTime(5),
                            EndDate = read.GetDateTime(6),
                            Salt = read.GetString(7),
                            IsDeleted = read.GetBoolean(8)
                        };
                    }
                    return user;
                }
            }
        }

        //--------- Items -------------

        /// <summary>
        /// Creating calendar
        /// </summary>
        /// <param name="day"></param>
        /// <param name="user"></param>
        public void CreateDay(Days day, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("CreateDay", sql);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@date", day.Date);
                cmd.Parameters.AddWithValue("@abscenseID", day.AbscenseID);
                cmd.Parameters.AddWithValue("@abscenseText", day.AbsenceText);
                cmd.Parameters.AddWithValue("@startDay", day.StartDay);
                cmd.Parameters.AddWithValue("@endDay", day.EndDay);
                cmd.Parameters.AddWithValue("@min", day.Min);
                cmd.Parameters.AddWithValue("@hour", day.Hour);
                cmd.Parameters.AddWithValue("@saldo", day.Saldo);
                cmd.Parameters.AddWithValue("@toMin", day.TotalMin);
                cmd.Parameters.AddWithValue("@toHour", day.TotalHour);
                cmd.Parameters.AddWithValue("@totalSaldo", day.TotalSaldo);

                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Method for creating user in name table
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public UserPersonalInfo CreateUserInfo(UserPersonalInfo info)
        {
            if (info.MiddleName == null)
            {
                info.MiddleName = "";
            }

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("CreateName", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@firstName", info.FirstName);
                cmd.Parameters.AddWithValue("@middleName", info.MiddleName);
                cmd.Parameters.AddWithValue("@lastName", info.LastName);
                cmd.Parameters.AddWithValue("@fullName", info.FullName);

                cmd.ExecuteNonQuery();
            }

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("GetUserInfo", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@fullName", info.FullName);


                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        info = new UserPersonalInfo
                        {
                            Name_ID = read.GetInt32(0)
                        };
                    }
                    return info;
                }
            }
        }

        /// <summary>
        /// Create a request for abscense
        /// </summary>
        /// <returns></returns>
        public void CreateRequest(Request request)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("CreateRequest", sql);
                cmd.CommandType= CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", request.UserID);
                cmd.Parameters.AddWithValue("@AbsID", request.AbsID);
                cmd.Parameters.AddWithValue("@Text", request.Text);
                cmd.Parameters.AddWithValue("@Date", request.Date);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create a request with two dates for abscense
        /// </summary>
        /// <returns></returns>
        public void CreateRequestTwoDates(Request request)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("CreateRequestTwoDates", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", request.UserID);
                cmd.Parameters.AddWithValue("@AbsID", request.AbsID);
                cmd.Parameters.AddWithValue("@Text", request.Text);
                cmd.Parameters.AddWithValue("@Date", request.Date);
                cmd.Parameters.AddWithValue("@SecDate", request.SecDate);

                cmd.ExecuteNonQuery();
            }
        }
        #endregion


        #region Update

        /// <summary>
        /// Method for updating users values in database
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(Users user)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateUser", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", user.User_ID);

                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Status", user.Status);
                cmd.Parameters.AddWithValue("@startDate", user.StartDate);
                cmd.Parameters.AddWithValue("@endDate", user.EndDate);


                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update day
        /// </summary>
        /// <param name="day"></param>
        public void UpdateDay(Days day)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateDay", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", day.Days_ID);
                cmd.Parameters.AddWithValue("@date", day.Date);
                cmd.Parameters.AddWithValue("@abscenseID", day.AbscenseID);
                cmd.Parameters.AddWithValue("@abscenseText", day.AbsenceText);
                cmd.Parameters.AddWithValue("@startDay", day.StartDay);
                cmd.Parameters.AddWithValue("@endDay", day.EndDay);
                cmd.Parameters.AddWithValue("@min", day.Min);
                cmd.Parameters.AddWithValue("@hour", day.Hour);
                cmd.Parameters.AddWithValue("@saldo", day.Saldo);
                cmd.Parameters.AddWithValue("@toMin", day.TotalMin);
                cmd.Parameters.AddWithValue("@toHour", day.TotalHour);
                cmd.Parameters.AddWithValue("@totalSaldo", day.TotalSaldo);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method for updating user's personal infomation
        /// </summary>
        /// <param name="person"></param>
        public void UpdateUserInfo(UserPersonalInfo person)
        {
            if (person.MiddleName == null)
            {
                person.MiddleName = "";
            }

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateUserInfo", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", person.Name_ID);

                cmd.Parameters.AddWithValue("@firstNavn", person.FirstName);
                cmd.Parameters.AddWithValue("@middleNavn", person.MiddleName);
                cmd.Parameters.AddWithValue("@lastNavn", person.LastName);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update request with one date
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        public void UpdateRequest(Request request, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateRequest", sql);
                cmd.CommandType= CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UserID", request.UserID);
                cmd.Parameters.AddWithValue("@AbsID", request.AbsID);
                cmd.Parameters.AddWithValue("@Text", request.Text);
                cmd.Parameters.AddWithValue("@Date", request.Date);
            }
        }

        /// <summary>
        /// Update request with two dates
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        public void UpdateRequestTwoDates(Request request, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateRequestTwoDates", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@UserID", request.UserID);
                cmd.Parameters.AddWithValue("@AbsID", request.AbsID);
                cmd.Parameters.AddWithValue("@Text", request.Text);
                cmd.Parameters.AddWithValue("@Date", request.Date);
                cmd.Parameters.AddWithValue("@SecDate", request.SecDate);

            }
        }

        /// <summary>
        /// Update answer of request
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bit"></param>
        public void UpdateRequestAnswered(int id, bool bit)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateRequestAnswered", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Answered", bit);
            }
        }

        /// <summary>
        /// Set abscens
        /// </summary>
        /// <param name="day"></param>
        public void UpdateAbscence(string text, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateAbscence", sql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                cmd.Parameters.AddWithValue("@AbscenceText", text);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method for Update Abscense type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dayID"></param>
        public void UpdateAbsencseType(int id, int dayID)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateAbsencseType", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@dayID", dayID);

                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Scan in date
        /// </summary>
        /// <param name="day"></param>
        public void UpdateStartDay(DateTime date, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateStart", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                cmd.Parameters.AddWithValue("@StartDay", date);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Scan out date
        /// </summary>
        /// <param name="day"></param>
        public void UpdateEndDay(DateTime date, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateEnd", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                cmd.Parameters.AddWithValue("@EndDay", date);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update saldo
        /// </summary>
        /// <param name="day"></param>
        public void UpdateSaldo(int min, int hour, string saldo, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateSaldo", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@min", min);
                cmd.Parameters.AddWithValue("@hour", hour);
                cmd.Parameters.AddWithValue("@Saldo", saldo);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Methoed for updating the total saldo
        /// </summary>
        /// <param name="day"></param>
        /// <param name="id"></param>
        public void UpdateTotalSaldo(int min, int hour, string saldo, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateTotalSaldo", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@totalMin", min);
                cmd.Parameters.AddWithValue("@totalHour", hour);
                cmd.Parameters.AddWithValue("@saldo", saldo);

                cmd.ExecuteNonQuery();
            }
        }

        #endregion


        #region Read

        /// <summary>
        /// Get request by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Request FindRequest(int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("FindRequest", sql);
                cmd.CommandType= CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", id);

                Request request = new Request();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        request = new Request()
                        {
                            RequestID = read.GetInt32(0),
                            UserID = read.GetInt32(1),
                            AbsID = read.GetInt32(2),
                            Text = read.GetString(3),
                            Date = read.GetDateTime(4),
                            Answer = read.GetBoolean(5),
                            SecDate = read.GetDateTime(6)
                        };
                    }
                    return request;
                }
            }
        }

        /// <summary>
        /// Read all users from database
        /// </summary>
        /// <param name="list"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public List<Users> ReadAllUsers()
        {
            List<Users> list = new List<Users>();

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("ReadAllUsers", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                Users users = new Users();


                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        users = new Users
                        {
                            User_ID = read.GetInt32(0),
                            UserName = read.GetString(1),
                            Password = read.GetString(2),
                            Name_ID = read.GetInt32(3),
                            Status = read.GetString(4),
                            StartDate = read.GetDateTime(5),
                            EndDate = read.GetDateTime(6),
                            Salt = read.GetString(7),
                            IsDeleted = read.GetBoolean(8)
                        };

                        list.Add(users);
                    }
                }
            }

            return list;
        }


        /// <summary>
        /// Method for getting user by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Users FindUser(int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("GetByID", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                Users user = new Users();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        user = new Users
                        {
                            User_ID = read.GetInt32(0),
                            UserName = read.GetString(1),
                            Password = read.GetString(2),
                            Name_ID = read.GetInt32(3),
                            Status = read.GetString(4),
                            StartDate = read.GetDateTime(5),
                            EndDate = read.GetDateTime(6),
                            Salt = read.GetString(7),
                            IsDeleted = read.GetBoolean(8)
                        };
                    }
                }

                return user;
            }
        }

        /// <summary>
        /// Method for finding all days by date
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<Days> FindAllDays(int id, DateTime date)
        {
            List<Days> days = new List<Days>();

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("FindAllDays", sql);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@date", date);

                Days day = new Days();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        day = new Days()
                        {
                            Days_ID = read.GetInt32(0),
                            UserID = read.GetInt32(1),
                            AbscenseID = read.GetInt32(2),
                            Date = read.GetDateTime(3),
                            AbsenceText = read.GetString(4),
                            StartDay = read.GetDateTime(5),
                            EndDay = read.GetDateTime(6),
                            Min = read.GetInt32(7),
                            Hour = read.GetInt32(8),
                            Saldo = read.GetString(9),
                            TotalMin = read.GetInt32(10),
                            TotalHour = read.GetInt32(11),
                            TotalSaldo = read.GetString(12)
                        };

                        days.Add(day);
                    }
                }
            }

            return days;
        }

        /// <summary>
        /// Method for finding all days
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Days> FindAllDaysByID(int id)
        {
            List<Days> days = new List<Days>();

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("FindAllDaysByID", sql);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                Days day = new Days();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        day = new Days()
                        {
                            Days_ID = read.GetInt32(0),
                            UserID = read.GetInt32(1),
                            AbscenseID = read.GetInt32(2),
                            Date = read.GetDateTime(3),
                            AbsenceText = read.GetString(4),
                            StartDay = read.GetDateTime(5),
                            EndDay = read.GetDateTime(6),
                            Min = read.GetInt32(7),
                            Hour = read.GetInt32(8),
                            Saldo = read.GetString(9),
                            TotalMin = read.GetInt32(10),
                            TotalHour = read.GetInt32(11),
                            TotalSaldo = read.GetString(12)
                        };

                        days.Add(day);
                    }
                }
            }

            return days;
        }

        /// <summary>
        /// Fidning day by date and id
        /// </summary>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Days FindDay(DateTime date, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("FindDay", sql);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@id", id);

                Days day = new Days();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        day = new Days()
                        {
                            Days_ID = read.GetInt32(0),
                            UserID = read.GetInt32(1),
                            AbscenseID = read.GetInt32(2),
                            Date = read.GetDateTime(3),
                            AbsenceText = read.GetString(4),
                            StartDay = read.GetDateTime(5),
                            EndDay = read.GetDateTime(6),
                            Min = read.GetInt32(7),
                            Hour = read.GetInt32(8),
                            Saldo = read.GetString(9),
                            TotalMin = read.GetInt32(10),
                            TotalHour = read.GetInt32(11),
                            TotalSaldo = read.GetString(12)
                        };
                    }
                }

                return day;
            }
        }


        /// <summary>
        /// Find day by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Days FindDayByID(int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("FindDayByID", sql);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                Days day = new Days();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        day = new Days()
                        {
                            Days_ID = read.GetInt32(0),
                            UserID = read.GetInt32(1),
                            AbscenseID = read.GetInt32(2),
                            Date = read.GetDateTime(3),
                            AbsenceText = read.GetString(4),
                            StartDay = read.GetDateTime(5),
                            EndDay = read.GetDateTime(6),
                            Min = read.GetInt32(7),
                            Hour = read.GetInt32(8),
                            Saldo = read.GetString(9),
                            TotalMin = read.GetInt32(10),
                            TotalHour = read.GetInt32(11),
                            TotalSaldo = read.GetString(12)
                        };
                    }
                }

                return day;
            }
        }

        /// <summary>
        /// Method for getting salt
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Users FindByUserName(string username)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("GetByUserName", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserName", username);

                Users user = new Users();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        user = new Users
                        {
                            Salt = read.GetString(0),
                            User_ID = read.GetInt32(1)
                        };
                    }
                    return user;
                }
            }
        }

        public UserPersonalInfo FindUserInfo(int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("FindUserInfo", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                UserPersonalInfo person = new UserPersonalInfo();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        person = new UserPersonalInfo
                        {
                            FirstName = read.GetString(1),
                            MiddleName = read.GetString(2),
                            LastName = read.GetString(3)
                        };
                    }
                }


                return person;
            }
        }

        /// <summary>
        /// Find day by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Days FindTotalSaldo()
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("FindTotalSaldo", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                Days days = new Days();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        days = new Days
                        {
                            Days_ID = read.GetInt32(0),
                            UserID = read.GetInt32(1),
                            AbscenseID = read.GetInt32(2),
                            Date = read.GetDateTime(3),
                            AbsenceText = read.GetString(4),
                            StartDay = read.GetDateTime(5),
                            EndDay = read.GetDateTime(6),
                            Min = read.GetInt32(7),
                            Hour = read.GetInt32(8),
                            Saldo = read.GetString(9),
                            TotalMin = read.GetInt32(10),
                            TotalHour = read.GetInt32(11),
                            TotalSaldo = read.GetString(12)
                        };
                    }
                }

                return days;
            }
        }

        /// <summary>
        /// Method for getting all abscenseType
        /// </summary>
        /// <returns></returns>
        public List<AbscenseType> GetAllAbscenseType()
        {
            List<AbscenseType> abscenseTypes = new List<AbscenseType>();

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("GetAllAbscenseTypes", sql);
                cmd.CommandType= CommandType.StoredProcedure;

                AbscenseType abscense = new AbscenseType();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        abscense = new AbscenseType
                        {
                            ID = read.GetInt32(0),
                            Type = read.GetString(1)
                        };

                        abscenseTypes.Add(abscense);
                    }
                }

                return abscenseTypes;
            }
        }

        /// <summary>
        /// Method for getting illegal abscense
        /// </summary>
        /// <returns></returns>
        public AbscenseType FindAbscenseByText(string text)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("FindAbscenseByText", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Text", text);

                AbscenseType abs = new AbscenseType();

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        abs = new AbscenseType()
                        {
                            ID = read.GetInt32(0),
                            Type= read.GetString(1)
                        };
                    }

                    return abs;
                }
            }
        }

        /// <summary>
        /// Method for getting abscense type 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<AbscenseType> GetAbscenseText()
        {
            List<AbscenseType> list = new List<AbscenseType>();

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("GetAbscenseText", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        AbscenseType abs = new AbscenseType
                        { 
                            ID = read.GetInt32(0),
                            Type = read.GetString(1)
                        };

                        list.Add(abs);
                    }
                }

                return list;                
            }  
        }

        #endregion 


        #region Delete

        /// <summary>
        /// Method for removing users from database
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand deleteUser = new SqlCommand("DeleteUser", sql);
                deleteUser.CommandType = CommandType.StoredProcedure;

                deleteUser.Parameters.AddWithValue("@id", id);

                deleteUser.ExecuteNonQuery();
            }
        }


        #endregion        
    }
}
