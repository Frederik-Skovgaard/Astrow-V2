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
        public void DeleteUser(Users user)
        {
            stored.DeleteUser(user);
        }

        /// <summary>
        /// Update user parameters
        /// </summary>
        /// <param name="user"></param>
        public  void UpdateUser(Users user)
        {
            stored.UpdateUser(user);
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
                    User_ID = user.User_ID
                };
            }
            else
            {
                return logedUser;
            }
        }


        /// <summary>
        /// Encryptor
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in algorithm.ComputeHash(plainTextWithSaltBytes))
            {
                sb.Append(b.ToString("X2"));
            }


            return sb.ToString();
        }
    }
}
