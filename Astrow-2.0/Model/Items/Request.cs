using System;

namespace Astrow_2._0.Model.Items
{
    public class Request
    {
        public Request()
        {

        }
        public Request(int requestID, int userID, int absID, string text, DateTime date)
        {
            this.RequestID = requestID;
            this.UserID = userID;
            this.AbsID = absID;
            this.Text = text;
            this.Date = date;
        }
        public Request(int requestID, int userID, int absID, string text, DateTime date, DateTime secDate)
        {
            this.RequestID = requestID;
            this.UserID = userID;
            this.AbsID = absID;
            this.Text = text;
            this.Date = date;
            this.SecDate = secDate;
        }

        public Request(int requestID, int userID, int absID, string text, DateTime date, DateTime secDate, bool answer)
        {
            this.RequestID = requestID;
            this.UserID = userID;
            this.AbsID = absID;
            this.Text = text;
            this.Date = date;
            this.SecDate = secDate;
            this.Answer = answer;
        }

        public int RequestID { get; set; }
        public int UserID { get; set; }
        public int AbsID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public DateTime SecDate { get; set; }
        public bool Answer { get; set; }
    }
}
