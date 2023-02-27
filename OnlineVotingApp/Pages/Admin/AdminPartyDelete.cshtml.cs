using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineVotingApp.Models;

namespace OnlineVotingApp.Pages.Admin
{
    public class AdminDeletePartyModel : PageModel
    {
        public List<VotingAppDetails> deleteIDList = new List<VotingAppDetails>();
        public VotingAppDetails deleteID = new VotingAppDetails();
        public void OnGet()
        {

        }
    }
}
