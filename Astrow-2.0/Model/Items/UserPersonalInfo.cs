using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model.Items
{
    public class UserPersonalInfo
    {
        public UserPersonalInfo()
        {

        }
        public UserPersonalInfo(int nameID, string firstname, string middlename, string lastname)
        {
            this.Name_ID = nameID;
            this.FirstName = firstname;
            this.MiddleName = middlename;
            this.LastName = lastname;
            this.FullName = $"{firstname} {middlename} {lastname}";
        }

        public int Name_ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }
}
