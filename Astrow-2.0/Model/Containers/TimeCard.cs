using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model.Containers
{
    public class TimeCard
    {
        public TimeCard()
        {

        }
        public TimeCard(int id, int dayID, int userID)
        {
            this.TimeCard_ID = id;
            this.Days_ID = dayID;
            this.User_ID = userID;
        }

        public int TimeCard_ID { get; set; }
        public int Days_ID { get; set; }
        public int User_ID { get; set; }
    }
}
