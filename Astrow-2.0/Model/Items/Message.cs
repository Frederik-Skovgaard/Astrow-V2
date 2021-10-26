using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model.Items
{
    public class Message
    {
        public Message()
        {

        }
        public Message(int id, string message, string sender, DateTime date)
        {
            this.Message_ID = id;
            this._Message = message;
            this.Sender = sender;
            this.Date = date;
        }

        public int Message_ID { get; set; }
        public string _Message { get; set; }
        public string Sender { get; set; }
        public DateTime Date { get; set; }
    }
}
