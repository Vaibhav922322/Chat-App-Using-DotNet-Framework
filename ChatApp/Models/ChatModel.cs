using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ChatApp.Models
{
    public class ChatModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string user1 { get; set; }

        [Required]
        public string user1name { get; set; }

        [Required]
        public string user2name { get; set; }

        [Required]
        public string user2 { get; set; }

        [Required]
        public DateTime timestamp { get; set; }
    }
}