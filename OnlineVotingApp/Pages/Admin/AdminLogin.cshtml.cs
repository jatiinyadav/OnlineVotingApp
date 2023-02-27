using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineVotingApp.Pages.Admin
{
    public class AdminLoginModel : PageModel
    {
        public string errorMessage = "";

        [BindProperty]
        public string? Username{ get; set; }
        public string? Password { get; set; }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            Username = Convert.ToString(Request.Form["adminUsername"]);
            Password = Convert.ToString(Request.Form["adminPassword"]);

            if(Username == "admin" && Password == "admin")
            {
                Response.Redirect("/Admin/AdminData");
            } else
            {
                errorMessage = "Incorrect";
            }
        }
    }
}
