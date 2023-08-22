using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class DisplayThings
    {

        public class User
        {
            public string userId { get; set; }
            public string userName { get; set; }
        }
        public class Chat
        {
            public string chatId { get; set; }
            public string personName { get; set; }
            public string receiverId { get; set; }
            public DateTime timestamp { get; set; }

        }

        public class Message
        {
            public string chatId { get; set; }
            public string receiverId { get; set; }
            public string text { get; set; }
        }


        public User user { get; set; }
        public Chat chat { get; set; }
        public Message message { get; set; }

    }
}