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
        public Days(int id, DateTime date, DateTime absence, DateTime regi, DateTime saldo, DateTime flex)
        {
            this.Days_ID = id;
            this.Date = date;
            this.Absence = absence;
            this.Registry = regi;
            this.Saldo = saldo;
            this.Flex = flex;
        }

        public int Days_ID { get; set; }
        public DateTime Date { get; set; }
        public DateTime Absence { get; set; }
        public DateTime Registry { get; set; }
        public DateTime Saldo { get; set; }
        public DateTime Flex { get; set; }
    }
}
