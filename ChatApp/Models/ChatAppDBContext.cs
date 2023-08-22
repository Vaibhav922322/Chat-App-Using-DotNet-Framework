using System;
using System.Data.Entity;
using System.Linq;

namespace ChatApp.Models
{
    public class ChatAppDBContext : DbContext
    {
        public ChatAppDBContext()
            : base("name=ChatAppDBString")
        {
        }

        public virtual DbSet<UserModel> userDBSet { get; set; }
        public virtual DbSet<ChatModel> chatDBSet { get; set; }
        public virtual DbSet<MessageModel> messageDBSet { get; set; }
    }
}