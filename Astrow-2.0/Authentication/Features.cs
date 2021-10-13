using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Astrow_2._0.Repository;
using Microsoft.Extensions.Configuration;
using Astrow_2._0.Model;
using Astrow_2._0.CustomExceptions;

namespace Astrow_2._0.Authentication
{
    public class Features : IFeatures
    {
        private readonly IUserRepository _UserRepository;

        public Features(IUserRepository repository)
        {
            _UserRepository = repository;
        }

        public static string CookieID { get; private set; }
        public static string LoginPath { get; private set; }
        public static string AccessDeniedPath { get; private set; }
        public static string ReturnURLParam { get; private set; }
        public static double CookieExpiration { get; private set; }

        public static void Register(IConfigurationRoot config)
        {
            CookieID = config.GetValue<string>("CookieName");
            LoginPath = config.GetValue<string>("LoginPath");
            AccessDeniedPath = config.GetValue<string>("AccessDenaidPath");
            ReturnURLParam = config.GetValue<string>("ReturnURLParama");
            CookieExpiration = config.GetValue<double>("ExpireTimeSpan");
        }

        public LogedUser ValidateUser(string username, string password)
        {
            try
            {
                LogedUser user = _UserRepository.Login(username, password);

                if (user != null)
                {
                    return new LogedUser(user.User_ID, user.UserName, user.Status);
                }
            }
            catch (LoginException e)
            {
                throw new LoginException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception("Login error!");
            }

            return null;
        }
    }
}
