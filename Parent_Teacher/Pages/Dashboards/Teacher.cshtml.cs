using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Parent_Teacher.Pages.Dashboards
{
    public class TeacherModel : PageModel
    {
        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Teacher")
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
