using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model
{
    public class Users
    {
        public Users()
        {

        }
        public Users(int userID, string userName, string password, int nameID, int inboxID, int timeCardID, int filesID, string status, bool isDeleted)
        {
            this.User_ID = userID;
            this.UserName = userName;
            this.Password = password;
            this.Name_ID = nameID;
            this.Inbox_ID = inboxID;
            this.TimeCard_ID = timeCardID;
            this.Files_ID = filesID;
            this.Status = status;
            this.IsDeleted = isDeleted;
        }

        public int User_ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Name_ID { get; set; }
        public int Inbox_ID { get; set; }
        public int TimeCard_ID { get; set; }
        public int Files_ID { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
