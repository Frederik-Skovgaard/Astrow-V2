using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Astrow_2._0.Model;

namespace Astrow_2._0.DataLayer
{

    public class StoredProcedure
    {
        string message = "Hej ny elev";

        internal static IConfigurationRoot configuration { get; set; }
        static string connectionString;

        SqlConnection sql = new SqlConnection(connectionString);

        public static void SetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            configuration = builder.Build();
            connectionString = configuration.GetConnectionString("AstrowDatabase");
        }



        /// <summary>
        /// Method for inserting users into databases
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(Users user, UserPersonalInfo info)
        {
            using (sql)
            {
                sql.Open();

                SqlCommand createDay = new SqlCommand("CreateDay", sql);
                createDay.CommandType = CommandType.StoredProcedure;

                createDay.Parameters.AddWithValue("@date", );
            }

            //Create new user message
            using (sql)
            {
                sql.Open();

                SqlCommand createMessage = new SqlCommand("CreateMessage", sql);
                createMessage.CommandType = CommandType.StoredProcedure;

                createMessage.Parameters.AddWithValue("@message", message);
                createMessage.Parameters.AddWithValue("@sender", "Auto-Message");

                createMessage.ExecuteNonQuery();

            }

            //Create personal info
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

            //Create user
            using (sql)
            {
                sql.Open();

                SqlCommand createUser = new SqlCommand("CreateUser", sql);
                createUser.CommandType = CommandType.StoredProcedure;

                createUser.Parameters.AddWithValue("@UserName", user.UserName);
                createUser.Parameters.AddWithValue("@Password", user.Password);
                createUser.Parameters.AddWithValue("@Status", user.Status);
                createUser.Parameters.AddWithValue("@salt", user.Salt);

                createUser.ExecuteNonQuery();
            }
        }


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
    }
}
