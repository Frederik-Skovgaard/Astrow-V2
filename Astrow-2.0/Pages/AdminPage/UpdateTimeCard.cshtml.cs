using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;

namespace Astrow_2._0.Pages.AdminPage
{
    public class UpdateTimeCardModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public UpdateTimeCardModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("_UserID") == 0)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                if (HttpContext.Session.GetString("_Status") != "Instructør")
                {
                    return RedirectToPage("/HomePage");
                }
                else
                {
                    return Page();
                }
            }
            
        }
    }
}
