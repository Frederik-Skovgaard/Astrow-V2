using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;

namespace Astrow_2._0.Authentication
{
    public interface IFeatures
    {
        LogedUser ValidateUser(string username, string password);
    }
}