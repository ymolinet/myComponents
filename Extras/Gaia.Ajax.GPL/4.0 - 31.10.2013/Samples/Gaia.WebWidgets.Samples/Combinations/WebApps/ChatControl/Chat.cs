namespace Gaia.WebWidgets.Samples.Combinations.WebApps.ChatControl
{
    using System;

    public class Chat
    {
        private int _id;
        private DateTime _when;
        private string _content;
        private string _user;

        public Chat(int id, string userName, string content)
        {
            _id = id;
            _when = DateTime.Now;
            _content = content;
            _user = userName;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public DateTime When
        {
            get { return _when; }
            set { _when = value; }
        }
    }
}