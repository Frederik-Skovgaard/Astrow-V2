using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Astrow_2._0.Model;

namespace Astrow_2._0.Repository
{
    interface IUserRepository
    {
        void CreateUser(Users user);

        void DeleteUser(Users user);

        void UpdateUser(Users user);

        List<Users> ReadAllUsers();

        Users FindUser(int id);

    }
}
