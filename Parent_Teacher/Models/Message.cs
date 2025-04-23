using System;
using System.ComponentModel.DataAnnotations;

namespace Parent_Teacher.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int SenderId { get; set; }       // Foreign key
        public User? Sender { get; set; }

        public int ReceiverId { get; set; }     // Foreign key
        public User? Receiver { get; set; }

        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }




}
