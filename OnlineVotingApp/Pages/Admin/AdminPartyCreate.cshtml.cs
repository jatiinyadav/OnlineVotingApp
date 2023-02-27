using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineVotingApp.Models;
using System.Data.SqlClient;

namespace OnlineVotingApp.Pages.Admin
{
    public class AdminPartyCreateModel : PageModel
    {
		public VotingAppDetails guestDetails = new VotingAppDetails();
		public void OnGet()
		{
		}

		public void OnPost()
		{
			guestDetails.PartyName = Convert.ToString(Request.Form["partyName"]);
			guestDetails.PartySymbol = Convert.ToString(Request.Form["partySymbol"]);
			try
			{
				string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

				using (SqlConnection cnn = new SqlConnection(connectionString))
				{
					cnn.Open();
					string query = $"INSERT INTO PartyDetails VALUES ('{guestDetails.PartyName}', '{guestDetails.PartySymbol}', 0)";
					using (SqlCommand cmd = new SqlCommand(query, cnn))
					{
						cmd.Parameters.AddWithValue("@partyName", guestDetails.PartyName);
						cmd.Parameters.AddWithValue("@partySymbol", guestDetails.PartySymbol);

						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{

			}

			Response.Redirect("/Admin/AdminPartyUpdate");
		}
	}
}
