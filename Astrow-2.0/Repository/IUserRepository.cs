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
        #region User
        void CreateUser(Users user, UserPersonalInfo info);

        void DeleteUser(int id);

        void UpdateUser(Users user);

        void UpdateUserInfo(UserPersonalInfo person);

        List<Users> ReadAllUsers();

        Users FindUser(int id);

        Users FindByUserName(string username);

        UserPersonalInfo FindUserInfo(int id);

        LogedUser Login(string username, string password);

        #endregion


        #region Encryption
        string CreateSalt(int size);

        string GenerateHash(string password, string salt);

        bool PasswordAreEqual(string plainTextPassword, string hashedPassword, string salt);

        #endregion

        #region Days
        void CreateDay(Days day, int id);

        List<Days> FindAllDays(int id, DateTime date);

        List<Days> FindAllDaysByID(int id);

        Days FindDay(DateTime date, int id);

        Days FindTotalSaldo();

        Days FindDayByID(int id);

        void UpdateDay(Days day);

        void UpdateStartDay(DateTime date, int id);

        void UpdateEndDay(DateTime date, int id);

        void UpdateSaldo(int min, int hour, string saldo, int id);

        void UpdateTotalSaldo(int min, int hour, string saldo, int id);

        void UpdateAbsence(DateTime date, string text, int id);

        void Registrer(int id);

        IEnumerable<DateTime> EachDay(DateTime from, DateTime thru);

        IEnumerable<DateTime> EachYear(DateTime from, DateTime thru);
        #endregion
    }
}
