using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;

namespace Parent_Teacher.Pages.Parent
{
    public class ChatModel : PageModel
    {
        private readonly AppDbContext _context;

        public ChatModel(AppDbContext context)
        {
            _context = context;
        }

        public List<User> UsersList { get; set; } = new();
        public List<Message> MessageList { get; set; } = new();

        public string CurrentUserEmail => HttpContext.Session.GetString("UserEmail") ?? "";
        public string CurrentUserRole => HttpContext.Session.GetString("UserRole") ?? "";
        public User? CurrentUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SelectedUserId { get; set; }

        [BindProperty]
        public string MessageContent { get; set; } = string.Empty;

        public void OnGet()
        {
            LoadUserData();

            if (CurrentUser == null || SelectedUserId == 0) return;

            var currentUserId = CurrentUser.Id;

            MessageList = _context.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == SelectedUserId)
                         || (m.SenderId == SelectedUserId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }

        private void LoadUserData()
        {
            CurrentUser = _context.Users.FirstOrDefault(u => u.Email == CurrentUserEmail);
            if (CurrentUser == null) return;

            string oppositeRole = CurrentUserRole == "Teacher" ? "Parent" : "Teacher";
            UsersList = _context.Users.Where(u => u.Role == oppositeRole).ToList();
        }

        public async Task<IActionResult> OnPostSendMessageAsync()
        {
            LoadUserData();
            if (CurrentUser == null || SelectedUserId == 0 || string.IsNullOrWhiteSpace(MessageContent))
                return new JsonResult(new { success = false, error = "Invalid input" });

            var newMessage = new Message
            {
                SenderId = CurrentUser.Id,
                ReceiverId = SelectedUserId,
                Content = MessageContent,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                success = true,
                timestamp = newMessage.Timestamp.ToString("g")
            });
        }

        public async Task<IActionResult> OnGetNewMessagesAsync(int selectedUserId, int lastMessageId)
        {
            if (CurrentUser == null)
                return new JsonResult(new { success = false, message = "Not logged in" });

            var newMessages = await _context.Messages
                .Where(m =>
                    ((m.SenderId == CurrentUser.Id && m.ReceiverId == selectedUserId) ||
                     (m.SenderId == selectedUserId && m.ReceiverId == CurrentUser.Id))
                    && m.Id > lastMessageId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            var messageDtos = newMessages.Select(m => new
            {
                id = m.Id,
                content = m.Content,
                senderId = m.SenderId,
                timestamp = m.Timestamp.ToString("g") // formatted
            });

            return new JsonResult(new { success = true, messages = messageDtos });
        }
    }
}
