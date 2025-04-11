using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Parent_Teacher.Pages.Dashboards
{
    public class ParentModel : PageModel
    {
        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Parent")
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
