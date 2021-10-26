using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model.Containers
{
    public class LogedUser
    {
        public LogedUser()
        {

        }
        public LogedUser(int userID, string userName, string status)
        {
            this.User_ID = userID;
            this.UserName = userName;
            this.Status = status;
        }


        public string UserName { get; set; }
        public string Status { get; set; }
        public int User_ID { get; set; }
    }
}
