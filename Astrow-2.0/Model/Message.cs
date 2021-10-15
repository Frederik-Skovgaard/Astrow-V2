using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astrow_2._0.Model
{
    public class Message
    {
        public Message()
        {

        }
        public Message(string message, string sender, DateTime date)
        {
            this._Message = message;
            this.Sender = sender;
            this.Date = date;
        }

        public string _Message { get; set; }
        public string Sender { get; set; }
        public DateTime Date { get; set; }
    }
}
