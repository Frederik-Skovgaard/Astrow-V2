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
        public Days(int id, int userID, DateTime date, DateTime startDate, DateTime endDate, int min, int hour, string saldo, string totalSaldo)
        {
            this.Days_ID = id;
            this.UserID = userID;
            this.Date = date;
            
            this.StartDay = startDate;
            this.EndDay = endDate;
            this.Min = min;
            this.Hour = hour;
            this.Saldo = saldo;
            this.TotalSaldo = totalSaldo;
        }

        public int Days_ID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public int Min { get; set; }
        public int Hour { get; set; }
        public string Saldo { get; set; }
        public string TotalSaldo { get; set; }
    }
}
