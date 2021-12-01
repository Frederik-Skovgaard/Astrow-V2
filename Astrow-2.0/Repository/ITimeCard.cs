using Astrow_2._0.Model.Items;
using Astrow_2._0.Model.Containers;
using System;
using System.Collections.Generic;

namespace Astrow_2._0.Repository
{
    public interface ITimeCard
    {
        void CreateDay(Days day, Users user);

        void UpdateStartDay(Days day);

        void UpdateEndDay(Days day);

        void UpdateSaldo(Days day);

        void UpdateAbsence(Days day);

        IEnumerable<DateTime> EachDay(DateTime from, DateTime thru);

        IEnumerable<DateTime> EachYear(DateTime from, DateTime thru);

    }
}
