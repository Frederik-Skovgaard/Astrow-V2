using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model.Containers
{
    public class InBox
    {
        public InBox()
        {

        }
        public InBox(int id, int mesID, int userID)
        {
            this.Inbox_ID = id;
            this.Message_ID = mesID;
            this.User_ID = userID;
        }

        public int Inbox_ID { get; set; }
        public int Message_ID { get; set; }
        public int User_ID { get; set; }
    }
}
