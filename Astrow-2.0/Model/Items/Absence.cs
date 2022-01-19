using System;

namespace Astrow_2._0.Model.Items
{
    public class Absence
    {
        public Absence()
        {

        }
        public Absence(DateTime absenceDate, string abscenceText)
        {
            this.AbsenceDate = absenceDate;
            this.AbscenceText = abscenceText;
        }

        public DateTime AbsenceDate { get; set; }
        public string AbscenceText { get; set; }
    }
}
