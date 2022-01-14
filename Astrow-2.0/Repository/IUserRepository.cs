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
        void CreateUser(Users user, UserPersonalInfo info);

        void DeleteUser(Users user);

        void UpdateUser(Users user);

        void UpdateUserInfo(UserPersonalInfo person);

        List<Users> ReadAllUsers();

        Users FindUser(int id);

        Users FindByUserName(string username);

        UserPersonalInfo FindUserInfo(int id);

        LogedUser Login(string username, string password);

        string CreateSalt(int size);

        string GenerateHash(string password, string salt);

        bool PasswordAreEqual(string plainTextPassword, string hashedPassword, string salt);

    }
}
