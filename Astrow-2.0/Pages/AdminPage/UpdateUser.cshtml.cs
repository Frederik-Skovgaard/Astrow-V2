using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Astrow_2._0.Pages.AdminPage
{
    public class UpdateUserModel : PageModel
    {

        private readonly IUserRepository _userRepository;

        public UpdateUserModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public List<Users> UserList { get; set; }

        [BindProperty]
        public string UserName { get; set; }


        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string MiddleName { get; set; }

        [BindProperty]
        public string LastName { get; set; }


        [BindProperty]
        public string Password { get; set; }


        [BindProperty]
        public string Role { get; set; }

        [BindProperty]
        public string StartDate { get; set; }

        [BindProperty]
        public string EndDate { get; set; }


        [BindProperty]
        public int ID { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("_Status") != "Instructør")
            {
                return RedirectToPage("/Login");
            }
            else
            {
                UserList = _userRepository.ReadAllUsers();

                return Page();
            }
        }

        public void OnPostUser()
        {
            if (ID != 0)
            {
                Users user = _userRepository.FindUser(ID);

                UserPersonalInfo person = _userRepository.FindUserInfo(ID);

                FirstName = person.FirstName;
                MiddleName = person.MiddleName;
                LastName = person.LastName;

                UserName = user.UserName;

                StartDate = user.StartDate.ToString("yyyy/MM/dd");
                EndDate = user.EndDate.ToString("yyyy/MM/dd");

                Role = user.Status.ToString();

                UserList = _userRepository.ReadAllUsers();

            }
            else
            {
                UserList = _userRepository.ReadAllUsers();
            }
        }

        public void OnPostUpdateUser()
        {

        }
    }
}
