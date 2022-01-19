using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model.Items
{
    public class Days
    {
        public Days()
        {

        }
        public Days(int id, int userID, DateTime date, DateTime startDate, DateTime endDate, DateTime saldo)
        {
            this.Days_ID = id;
            this.User_ID = userID;
            this.Date = date;
            
            this.StartDay = startDate;
            this.EndDay = endDate;
            this.Saldo = saldo;
        }

        public int Days_ID { get; set; }
        public int User_ID { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public DateTime Saldo { get; set; }
    }
}
