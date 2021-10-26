using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model.Items
{
    public class Files
    {
        public Files()
        {

        }
        public Files(int id, string name, string type, string detail, string desc, DateTime date, bool sensitive)
        {
            this.File_ID = id;
            this.Name = name;
            this.Type = type;
            this.Details = detail;
            this.Description = desc;
            this.Date = date;
            this.SensitiveData = sensitive;
        }

        public int File_ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool SensitiveData { get; set; }
    }
}
