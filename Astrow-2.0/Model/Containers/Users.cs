using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model.Containers
{
    public class Users
    {
        public Users()
        {

        }
        public Users(int userID, string userName, string password, int nameID, string status, bool isDeleted, string salt, DateTime startDate, DateTime endDate)
        {
            this.User_ID = userID;
            this.UserName = userName;
            this.Password = password;
            this.Name_ID = nameID;
            this.Status = status;
            this.IsDeleted = isDeleted;
            this.Salt = salt;
            this.StartDate = startDate;
            this.EndDate = endDate;

        }

        public int User_ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Name_ID { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public string Salt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
