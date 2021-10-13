using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Astrow_2._0.Model;
using Astrow_2._0.DataLayer;
using Astrow_2._0.CustomExceptions;

namespace Astrow_2._0.Repository
{
    public class UserRepository : IUserRepository
    {
        StoredProcedure Stored = new StoredProcedure();

        List<Users> userList = new List<Users>();

        Users user = new Users();

        LogedUser loged = new LogedUser();

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(Users user)
        {
            Stored.CreateUser(user);
        }

        /// <summary>
        /// Delete user from database
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(Users user)
        {
            Stored.DeleteUser(user);
        }

        /// <summary>
        /// Update user parameters
        /// </summary>
        /// <param name="user"></param>
        public  void UpdateUser(Users user)
        {
            Stored.UpdateUser(user);
        }

        /// <summary>
        /// Return all users to list
        /// </summary>
        /// <returns></returns>
        public List<Users> ReadAllUsers()
        {
            if (userList.Count != 0)
            {
                userList.Clear();
            }

            userList = Stored.ReadAllUsers(userList, user);

            return userList;
        }

        /// <summary>
        /// Find User by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Users FindUser(int id)
        {
            user = Stored.FindByID(id, user);
            return user;
        }



        public LogedUser Login(string username, string password)
        {
            user = userList.SingleOrDefault(x => x.UserName == username && x.Password == password);

            if (user != null)
            {
                return loged = new LogedUser
                {
                    UserName = user.UserName,
                    Status = user.Status,
                    User_ID = user.User_ID
                };
            }


            throw new LoginException("Username or password does not match...");
        }
    }
}
