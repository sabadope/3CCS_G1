using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace Parent_Teacher.Pages.Parent
{
    public class ChatModel : PageModel
    {
        [BindProperty]
        public string Sender { get; set; }

        [BindProperty]
        public string Content { get; set; }

        public List<Message> Messages { get; set; } = new List<Message>();

        public string CurrentUser { get; set; } = "Parent1"; // Can change to "Teacher1" if needed

        private static List<Message> MessageStore = new List<Message>(); // Shared message list

        public void OnGet()
        {
            Messages = MessageStore;
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(Sender) && !string.IsNullOrEmpty(Content))
            {
                MessageStore.Add(new Message
                {
                    Sender = Sender,
                    Content = Content,
                    Timestamp = DateTime.Now
                });
            }

            return RedirectToPage();
        }

        public class Message
        {
            public string Sender { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}
