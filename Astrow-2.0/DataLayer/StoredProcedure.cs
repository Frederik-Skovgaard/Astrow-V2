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
    //TODO: Make CRUD procedure's

    public class StoredProcedure
    {
        //SQL connection
        #region SQL
        internal static IConfigurationRoot configuration { get; set; }
        static string connectionString;

        SqlConnection sql = new SqlConnection(connectionString);

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




        //Create Procedures
        #region Create


        /// <summary>
        /// Method for inserting users into databases
        /// </summary>
        /// <param name="user"></param>
        public void CreateUsers(Users user, Days day, UserPersonalInfo info, InBox inbox, FileBox fileBox, TimeCard timeCard)
        {
            CreateDay(day, user);

            CreateUserInfo(info);

            CreateInBox(inbox);

            CreateFile(fileBox);

            CreateTimeCard(timeCard);

            CreateUser(user);

            UpdateForgeignKey(user);
        }

        //--------- Container ---------

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(Users user)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand createUser = new SqlCommand("CreateUser", sql);
                createUser.CommandType = CommandType.StoredProcedure;

                createUser.Parameters.AddWithValue("@UserName", user.UserName);
                createUser.Parameters.AddWithValue("@Password", user.Password);
                createUser.Parameters.AddWithValue("@Status", user.Status);
                createUser.Parameters.AddWithValue("@salt", user.Salt);
                createUser.Parameters.AddWithValue("@startDate", user.StartDate);
                createUser.Parameters.AddWithValue("@endDate", user.EndDate);

                createUser.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create Time Card
        /// </summary>
        /// <param name="time"></param>
        public void CreateTimeCard(TimeCard time)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand createTime = new SqlCommand("CreateTimeCard", sql);
                createTime.CommandType = CommandType.StoredProcedure;

                createTime.Parameters.AddWithValue("@days", time.Days_ID);

                createTime.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create Inbox
        /// </summary>
        /// <param name="inbox"></param>
        public void CreateInBox(InBox inbox)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand createInbox = new SqlCommand("CreateInBox", sql);
                createInbox.CommandType = CommandType.StoredProcedure;

                createInbox.Parameters.AddWithValue("@messageID", inbox.Message_ID);

                createInbox.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Create File Box
        /// </summary>
        /// <param name="filebox"></param>
        public void CreateFile(FileBox filebox)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand createFileBox = new SqlCommand("CreateFiles", sql);
                createFileBox.CommandType = CommandType.StoredProcedure;

                createFileBox.Parameters.AddWithValue("@file_ID", filebox.File_ID);

                createFileBox.ExecuteNonQuery();
            }
        }

        //--------- Items -------------

        /// <summary>
        /// Creating calendar
        /// </summary>
        /// <param name="day"></param>
        /// <param name="user"></param>
        public void CreateDay(Days day, Users user)
        {
            using (sql)
            {
                sql.Open();


                foreach (DateTime date in EachDay(user.StartDate, user.EndDate))
                {
                    SqlCommand createDay = new SqlCommand("CreateDay", sql);

                    createDay.CommandType = CommandType.StoredProcedure;

                    createDay.Parameters.AddWithValue("@date", date.Date);
                    createDay.Parameters.AddWithValue("@absence", day.Absence);
                    createDay.Parameters.AddWithValue("@registry", day.Registry);
                    createDay.Parameters.AddWithValue("@saldo", day.Saldo);
                    createDay.Parameters.AddWithValue("@flex", day.Flex);

                    createDay.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Create a file
        /// </summary>
        /// <param name="file"></param>
        public void CreateFiles(Files file)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand createFile = new SqlCommand("CreateFile", sql);
                createFile.CommandType = CommandType.StoredProcedure;

                createFile.Parameters.AddWithValue("@Name", file.Name);
                createFile.Parameters.AddWithValue("@Type", file.Type);
                createFile.Parameters.AddWithValue("@Details", file.Details);
                createFile.Parameters.AddWithValue("@Description", file.Description);
                createFile.Parameters.AddWithValue("@Date", file.Date);
                createFile.Parameters.AddWithValue("@SensitiveDate", file.SensitiveData);

                createFile.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create Message
        /// </summary>
        /// <param name="mess"></param>
        public void CreateMessage(Message mess)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand createMessage = new SqlCommand("CreateMessage", sql);
                createMessage.CommandType = CommandType.StoredProcedure;

                createMessage.Parameters.AddWithValue("@message", mess._Message);
                createMessage.Parameters.AddWithValue("@sender", "Auto-Message");

                createMessage.ExecuteNonQuery();

            }
        }

        public void CreateUserInfo(UserPersonalInfo info)
        {
            using (sql)
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
        }

        #endregion

        //Update Procedures
        #region Update

        /// <summary>
        /// Method for updating users values in database
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(Users user)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand updateUser = new SqlCommand("UpdateUser", sql);
                updateUser.CommandType = CommandType.StoredProcedure;

                updateUser.Parameters.AddWithValue("@id", user.User_ID);

                updateUser.Parameters.AddWithValue("@UserName", user.UserName);
                updateUser.Parameters.AddWithValue("@Password", user.Password);
                updateUser.Parameters.AddWithValue("@Status", user.Status);

                updateUser.ExecuteNonQuery();
            }  
        }
        
        /// <summary>
        /// Give user the right foregine keys
        /// </summary>
        /// <param name="user"></param>
        public void UpdateForgeignKey(Users user)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand updateForgeinKeys = new SqlCommand("UpdateForgienkeys", sql);
                updateForgeinKeys.CommandType = CommandType.StoredProcedure;

                updateForgeinKeys.Parameters.AddWithValue("@id", user.User_ID);

                updateForgeinKeys.ExecuteNonQuery();
            }
        }

        #endregion

        //Delete Procedures
        #region Delete

        /// <summary>
        /// Method for removing users from database
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(Users user)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand deleteUser = new SqlCommand("DeleteUser", sql);
                deleteUser.CommandType = CommandType.StoredProcedure;

                deleteUser.Parameters.AddWithValue("@id", user.User_ID);

                deleteUser.ExecuteNonQuery();
            }        
        }


        #endregion

        //Read Procedures
        #region Read

        /// <summary>
        /// Read all users from database
        /// </summary>
        /// <param name="list"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public List<Users> ReadAllUsers(List<Users> list, Users users)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand readAll = new SqlCommand("ReadAllUsers", sql);
                readAll.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader read = readAll.ExecuteReader())
                {
                    while (read.Read())
                    {
                        users = new Users
                        {
                            User_ID = read.GetInt32(0),
                            UserName = read.GetString(1),
                            Password = (byte[])read[2],
                            Name_ID = read.GetInt32(3),
                            Inbox_ID = read.GetInt32(4),
                            TimeCard_ID = read.GetInt32(5),
                            Files_ID = read.GetInt32(6),
                            Status = read.GetString(7),
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
        public Users FindByID(int id, Users user)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand find = new SqlCommand("GetByID", sql);
                find.CommandType = CommandType.StoredProcedure;

                find.Parameters.AddWithValue("@id", id);

                using (SqlDataReader read = find.ExecuteReader())
                {
                    return user = new Users
                    {
                        User_ID = read.GetInt32(0),
                        UserName = read.GetString(1),
                        Password = (byte[])read[2],
                        Name_ID = read.GetInt32(3),
                        Inbox_ID = read.GetInt32(4),
                        TimeCard_ID = read.GetInt32(5),
                        Files_ID = read.GetInt32(6),
                        Status = read.GetString(7),
                        IsDeleted = read.GetBoolean(8)
                    };
                }
            }
        }


        /// <summary>
        /// Method for getting salt
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Users FindByUserName(string username, Users user)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand find = new SqlCommand("GetByUserName", sql);
                find.CommandType = CommandType.StoredProcedure;

                find.Parameters.AddWithValue("@UserName", username);

                using (SqlDataReader read = find.ExecuteReader())
                {
                    return user = new Users
                    {
                        Salt = read.GetString(0)
                    };
                }
            }
        }

        #endregion



        /// <summary>
        /// Method for getting days between 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="thru"></param>
        /// <returns></returns>
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }
    }
}
