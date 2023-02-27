using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineVotingApp.Models;
using System.Data.SqlClient;

namespace OnlineVotingApp.Pages.Admin
{
    public class AdminPartyUpdateModel : PageModel
    {
		public List<VotingAppDetails> guestDetailList = new List<VotingAppDetails>();
		public void OnGet()
        {
			string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

			using (SqlConnection cnn = new SqlConnection(connectionString))
			{
				cnn.Open();
				string query = "SELECT * FROM PartyDetails";
				using (SqlCommand cmd = new SqlCommand(query, cnn))
				{
					using (SqlDataReader dataReader = cmd.ExecuteReader())
					{
						while (dataReader.Read())
						{
							VotingAppDetails guestDetails = new VotingAppDetails();
							guestDetails.PartyID = dataReader.GetInt32(0);
							guestDetails.PartyName = dataReader.GetString(1);
							guestDetails.PartySymbol = dataReader.GetString(2);

							guestDetailList.Add(guestDetails);
						}
					}
				}
			}
		}
    }
}
