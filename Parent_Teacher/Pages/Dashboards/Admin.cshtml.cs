using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Parent_Teacher.Pages.Dashboards
{
    public class AdminModel : PageModel
    {
        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
