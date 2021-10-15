using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model
{
    public class UserPersonalInfo
    {
        public UserPersonalInfo()
        {

        }
        public UserPersonalInfo(string firstname, string middlename, string lastname)
        {
            this.FirstName = firstname;
            this.MiddleName = middlename;
            this.LastName = lastname;
            this.FullName = $"{firstname} {middlename} {lastname}";
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }
}
