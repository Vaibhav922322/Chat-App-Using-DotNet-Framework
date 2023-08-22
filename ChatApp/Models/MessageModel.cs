using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class MessageModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string chatId { get; set; }

        [Required]
        public string senderId { get; set; }

        [Required]
        public string recieverId { get; set; }

        [MinLength(1)]
        public string text { get; set; }

        [Required]
        public DateTime timestamp { get; set; }
    }
}