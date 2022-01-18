namespace Astrow_2._0.Model.Containers
{
    public class PersonalInfo
    {
        public PersonalInfo()
        {
            
        }
        public PersonalInfo(int id, string username, string status, string firstname, string middlename, string lastname)
        {
            this.ID = id;
            this.UserName = username;
            this.Status = status;
            this.FirstName = firstname;
            this.MiddleName = middlename;
            this.LastName = lastname;
        }

        public int ID { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
