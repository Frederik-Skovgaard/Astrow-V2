using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Model.Containers;

namespace Astrow_2._0.Repository
{
    public interface IUserRepository
    {
        void CreateUser(Users user, Days day, UserPersonalInfo info);

        void DeleteUser(Users user);

        void UpdateUser(Users user);

        List<Users> ReadAllUsers();

        Users FindUser(int id);

        Users FindByUserName(string username);

        LogedUser Login(string username, string password);

        string GenerateSaltedHash(byte[] plainText, byte[] salt);
    }
}
