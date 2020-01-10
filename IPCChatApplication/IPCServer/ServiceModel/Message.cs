using System;
using System.Collections.Generic;
using System.Text;

namespace IPCChatApplication.Shared
{
    public class Message
    {
        string user = "Anonymous";
        string text = "";
        DateTime time = DateTime.MinValue;

        public Message() { }

        public Message(string user, string text)
        {
            this.user = user;
            this.text = text;
        }

        public Message(string user, string text, DateTime time)
        {
            this.user = user;
            this.text = text;
            this.time = time;
        }

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
