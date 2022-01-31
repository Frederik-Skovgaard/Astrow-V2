using Astrow_2._0.Model.Items;
using Astrow_2._0.Model.Containers;
using System;
using System.Collections.Generic;

namespace Astrow_2._0.Repository
{
    public interface ITimeCard
    {
        void CreateDay(Days day, int id);

        List<Days> FindAllDays(int id, DateTime date);

        List<Days> FindAllDaysByID(int id);

        Days FindDay(DateTime date, int id);

        void UpdateStartDay(Days day, int id);

        void UpdateEndDay(Days day, int id);

        void UpdateSaldo(Days day, int id);

        void UpdateTotalSaldo(Days day, int id);

        void UpdateAbsence(Absence abs, Days day);

        void Registrer(int id);

        IEnumerable<DateTime> EachDay(DateTime from, DateTime thru);

        IEnumerable<DateTime> EachYear(DateTime from, DateTime thru);


    }
}
