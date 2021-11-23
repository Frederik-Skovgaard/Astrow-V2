using Astrow_2._0.Model.Items;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.DataLayer;
namespace Astrow_2._0.Repository
{
    public class TimeCard : ITimeCard
    {
        StoredProcedure stored = new StoredProcedure();

        public void CreateDay(Days day, Users user)
        {
            stored.CreateDay(day, user);
        }

        public void UpdateStartDay(Days day)
        {

        }

        public void UpdateEndDay(Days day)
        {

        }

        public void UpdateSaldo(Days day)
        {

        }

        public void UpdateAbsence(Days day)
        {

        }

    }
}
