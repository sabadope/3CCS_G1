using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Parent_Teacher.Pages.Admin
{
    public class LandingPageModel : PageModel
    {
        [BindProperty]
        public string? Email { get; set; }


        public void OnGet()
        {
        }

        public IActionResult OnPostSubscribe()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // You can save the email to the database or do something with it
            // For now just redirect back to same page
            return RedirectToPage("LandingPage");
        }
    }
}
