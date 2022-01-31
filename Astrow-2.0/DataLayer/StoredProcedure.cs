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

                SqlCommand createUser = new SqlCommand("CreateUser", sql);
                createUser.CommandType = CommandType.StoredProcedure;

                createUser.Parameters.AddWithValue("@id", info.Name_ID);
                createUser.Parameters.AddWithValue("@UserName", user.UserName);
                createUser.Parameters.AddWithValue("@Password", user.Password);
                createUser.Parameters.AddWithValue("@Status", user.Status);
                createUser.Parameters.AddWithValue("@salt", user.Salt);
                createUser.Parameters.AddWithValue("@startDate", user.StartDate);
                createUser.Parameters.AddWithValue("@endDate", user.EndDate);

                createUser.ExecuteNonQuery();
            }

            //Find ID
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand findUser = new SqlCommand("GetUser", sql);
                findUser.CommandType = CommandType.StoredProcedure;

                findUser.Parameters.AddWithValue("@userName", user.UserName);
                findUser.Parameters.AddWithValue("@password", user.Password);

                using (SqlDataReader read = findUser.ExecuteReader())
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

                SqlCommand createDay = new SqlCommand("CreateDay", sql);

                createDay.CommandType = CommandType.StoredProcedure;

                createDay.Parameters.AddWithValue("@id", id);
                createDay.Parameters.AddWithValue("@date", day.Date);
                createDay.Parameters.AddWithValue("@startDay", day.StartDay);
                createDay.Parameters.AddWithValue("@endDay", day.EndDay);
                createDay.Parameters.AddWithValue("@saldo", day.Saldo);
                createDay.Parameters.AddWithValue("@totalSaldo", day.TotalSaldo);

                createDay.ExecuteNonQuery();

            }
        }


        public UserPersonalInfo CreateUserInfo(UserPersonalInfo info)
        {
            if (info.MiddleName == null)
            {
                info.MiddleName = "";
            }

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand createName = new SqlCommand("CreateName", sql);
                createName.CommandType = CommandType.StoredProcedure;

                createName.Parameters.AddWithValue("@firstName", info.FirstName);
                createName.Parameters.AddWithValue("@middleName", info.MiddleName);
                createName.Parameters.AddWithValue("@lastName", info.LastName);
                createName.Parameters.AddWithValue("@fullName", info.FullName);

                createName.ExecuteNonQuery();
            }

            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand find = new SqlCommand("GetUserInfo", sql);
                find.CommandType = CommandType.StoredProcedure;

                find.Parameters.AddWithValue("@fullName", info.FullName);


                using (SqlDataReader read = find.ExecuteReader())
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

                SqlCommand updateUser = new SqlCommand("UpdateUser", sql);
                updateUser.CommandType = CommandType.StoredProcedure;

                updateUser.Parameters.AddWithValue("@id", user.User_ID);

                updateUser.Parameters.AddWithValue("@UserName", user.UserName);
                updateUser.Parameters.AddWithValue("@Password", user.Password);
                updateUser.Parameters.AddWithValue("@Status", user.Status);
                updateUser.Parameters.AddWithValue("@startDate", user.StartDate);
                updateUser.Parameters.AddWithValue("@endDate", user.EndDate);


                updateUser.ExecuteNonQuery();
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

                SqlCommand updateUserInfo = new SqlCommand("UpdateUserInfo", sql);
                updateUserInfo.CommandType = CommandType.StoredProcedure;

                updateUserInfo.Parameters.AddWithValue("@id", person.Name_ID);

                updateUserInfo.Parameters.AddWithValue("@firstNavn", person.FirstName);
                updateUserInfo.Parameters.AddWithValue("@middleNavn", person.MiddleName);
                updateUserInfo.Parameters.AddWithValue("@lastNavn", person.LastName);

                updateUserInfo.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Set abscens
        /// </summary>
        /// <param name="day"></param>
        public void UpdateAbscence(Absence abs, Days day)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand UpdateAbscence = new SqlCommand("UpdateAbscence", sql);
                UpdateAbscence.CommandType = CommandType.StoredProcedure;
                UpdateAbscence.Parameters.AddWithValue("@id", day.Days_ID);

                UpdateAbscence.Parameters.AddWithValue("@AbscenceDate", abs.AbsenceDate);
                UpdateAbscence.Parameters.AddWithValue("@AbscenceText", abs.AbscenceText);

                UpdateAbscence.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Scan in date
        /// </summary>
        /// <param name="day"></param>
        public void UpdateStartDay(Days day, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand scanIn = new SqlCommand("UpdateStart", sql);
                scanIn.CommandType = CommandType.StoredProcedure;

                scanIn.Parameters.AddWithValue("@id", id);

                scanIn.Parameters.AddWithValue("@StartDay", day.StartDay);

                scanIn.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Scan out date
        /// </summary>
        /// <param name="day"></param>
        public void UpdateEndDay(Days day, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand scanIn = new SqlCommand("UpdateEnd", sql);
                scanIn.CommandType = CommandType.StoredProcedure;

                scanIn.Parameters.AddWithValue("@id", id);

                scanIn.Parameters.AddWithValue("@EndDay", day.EndDay);

                scanIn.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update saldo
        /// </summary>
        /// <param name="day"></param>
        public void UpdateSaldo(Days day, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand saldo = new SqlCommand("UpdateSaldo", sql);
                saldo.CommandType = CommandType.StoredProcedure;

                saldo.Parameters.AddWithValue("@id", id);
                saldo.Parameters.AddWithValue("@Saldo", day.Saldo);

                saldo.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Methoed for updating the total saldo
        /// </summary>
        /// <param name="day"></param>
        /// <param name="id"></param>
        public void UpdateTotalSaldo(Days day, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand cmd = new SqlCommand("UpdateTotalSaldo", sql);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Saldo", day.Saldo);

                cmd.ExecuteNonQuery();
            }
        }

        #endregion


        #region Read

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

                SqlCommand readAll = new SqlCommand("ReadAllUsers", sql);
                readAll.CommandType = CommandType.StoredProcedure;

                Users users = new Users();


                using (SqlDataReader read = readAll.ExecuteReader())
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
        public Users FindByID(int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand find = new SqlCommand("GetByID", sql);
                find.CommandType = CommandType.StoredProcedure;

                find.Parameters.AddWithValue("@id", id);

                Users user = new Users();

                using (SqlDataReader read = find.ExecuteReader())
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

                SqlCommand find = new SqlCommand("FindAllDays", sql);

                find.CommandType = CommandType.StoredProcedure;

                find.Parameters.AddWithValue("@id", id);
                find.Parameters.AddWithValue("@date", date);

                Days day = new Days();

                using (SqlDataReader read = find.ExecuteReader())
                {
                    while (read.Read())
                    {
                        day = new Days()
                        {
                            Days_ID = read.GetInt32(0),
                            UserID = read.GetInt32(1),
                            Date = read.GetDateTime(2),
                            StartDay = read.GetDateTime(3),
                            EndDay = read.GetDateTime(4),
                            Saldo = read.GetString(5)
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

                SqlCommand find = new SqlCommand("FindAllDaysByID", sql);

                find.CommandType = CommandType.StoredProcedure;

                find.Parameters.AddWithValue("@id", id);

                Days day = new Days();

                using (SqlDataReader read = find.ExecuteReader())
                {
                    while (read.Read())
                    {
                        day = new Days()
                        {
                            Days_ID = read.GetInt32(0),
                            UserID = read.GetInt32(1),
                            Date = read.GetDateTime(2),
                            StartDay = read.GetDateTime(3),
                            EndDay = read.GetDateTime(4),
                            Saldo = read.GetString(5)
                        };

                        days.Add(day);
                    }
                }
            }

            return days;
        }

        public Days FindDay(DateTime date, int id)
        {
            using (sql = new SqlConnection(connectionString))
            {
                sql.Open();

                SqlCommand find = new SqlCommand("FindDay", sql);

                find.CommandType = CommandType.StoredProcedure;

                find.Parameters.AddWithValue("@Date", date);
                find.Parameters.AddWithValue("@id", id);

                Days day = new Days();

                using (SqlDataReader read = find.ExecuteReader())
                {
                    while (read.Read())
                    {
                        day = new Days()
                        {
                            Date = read.GetDateTime(0),
                            StartDay = read.GetDateTime(1)
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

                SqlCommand find = new SqlCommand("GetByUserName", sql);
                find.CommandType = CommandType.StoredProcedure;

                find.Parameters.AddWithValue("@UserName", username);

                Users user = new Users();

                using (SqlDataReader read = find.ExecuteReader())
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

                SqlCommand find = new SqlCommand("FindUserInfo", sql);
                find.CommandType = CommandType.StoredProcedure;

                find.Parameters.AddWithValue("@id", id);

                UserPersonalInfo person = new UserPersonalInfo();

                using (SqlDataReader read = find.ExecuteReader())
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
