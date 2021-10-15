using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model
{
    public class Days
    {
        public Days()
        {

        }
        public Days(DateTime date, DateTime absence, DateTime regi, DateTime saldo, DateTime flex)
        {
            this.Date = date;
            this.Absence = absence;
            this.Registry = regi;
            this.Saldo = saldo;
            this.Flex = flex;
        }

        public DateTime Date { get; set; }
        public DateTime Absence { get; set; }
        public DateTime Registry { get; set; }
        public DateTime Saldo { get; set; }
        public DateTime Flex { get; set; }
    }
}
