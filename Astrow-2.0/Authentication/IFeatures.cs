using Astrow_2._0.Model;

namespace Astrow_2._0.Authentication
{
    public interface IFeatures
    {
        LogedUser ValidateUser(string username, string password);
    }
}