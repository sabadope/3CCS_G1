using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;

namespace Parent_Teacher.Pages.Admin
{
    public class ChatModel : PageModel
    {
        private readonly AppDbContext _context;

        public ChatModel(AppDbContext context)
        {
            _context = context;
            NewMessage = new Message
            {
                Sender = string.Empty,
                Receiver = string.Empty,
                Content = string.Empty,
                Timestamp = DateTime.Now
            };
        }

        [BindProperty]
        public Message NewMessage { get; set; }

        public List<Message> Messages { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadMessagesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(NewMessage?.Content))
            {
                NewMessage.Sender = User.Identity?.Name ?? "Unknown";
                NewMessage.Timestamp = DateTime.Now;

                _context.Messages.Add(NewMessage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        private async Task LoadMessagesAsync()
        {
            var currentUser = User.Identity?.Name ?? "Unknown";

            Messages = await _context.Messages
                .Where(m => m.Receiver == currentUser)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

    }
}
