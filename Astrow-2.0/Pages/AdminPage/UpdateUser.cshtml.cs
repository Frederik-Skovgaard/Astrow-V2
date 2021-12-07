using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;

namespace Astrow_2._0.Pages.AdminPage
{
    public class UpdateUserModel : PageModel
    {

        private readonly IUserRepository _userRepository;

        public UpdateUserModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [BindProperty, Required(ErrorMessage = "* Fornavn skal udfyldes")]
        public string FirstName { get; set; }

        [BindProperty, Required(ErrorMessage = "* Efternavn skal udfyldes")]
        public string LastName { get; set; }

        [BindProperty, Required(ErrorMessage = "* Kodeord skal udfyldes")]
        public string Password { get; set; }

        [BindProperty, Required(ErrorMessage = "* Rolle skal udfyldes")]
        public string Role { get; set; }

        [BindProperty, Required(ErrorMessage = "* Start dato skal udfyldes")]
        public string StartDate { get; set; }

        [BindProperty, Required(ErrorMessage = "* Slut dato skal udfyldes")]
        public string EndDate { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("_Status") != "Instruct�r")
            {
                return RedirectToPage("/Login");
            }
            else
            {
                return Page();
            }
        }
    }
}
