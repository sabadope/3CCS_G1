﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Parent_Teacher.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }


}
