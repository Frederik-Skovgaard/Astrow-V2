using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astrow_2._0.Model.Containers;
using Astrow_2._0.Model.Items;
using Astrow_2._0.Repository;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Astrow_2._0.Pages.AdminPage
{
    public class DeleteUserModel : PageModel
    {

        private readonly IUserRepository _userRepository;

        public DeleteUserModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public List<Users> UserList { get; set; }

        [BindProperty]
        public int ID { get; set; }

        /// <summary>
        /// Check if users has rights to be on side
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            //Check if user has "Instruct�r" rights
            if (HttpContext.Session.GetString("_Status") != "Instruct�r")
            {
                return RedirectToPage("/Login");
            }
            else
            {
                //Fills dropdown with users 
                UserList = _userRepository.ReadAllUsers();

                return Page();
            }
        }

        public IActionResult OnPost(int id)
        {
            //Get user
            Users user = _userRepository.FindUser(id);

            //Fill user with new data
            user = new Users
            {
                User_ID = id,
                IsDeleted = true
            };

            //Check user of as deleted in database
            _userRepository.DeleteUser(user);


            //Popup message for succes
            ViewData["Message"] = string.Format("Bruger blev slettet...");

            return RedirectToPage("/AdminPage/DeleteUser");
        }


    }
}
