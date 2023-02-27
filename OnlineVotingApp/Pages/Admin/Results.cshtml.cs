using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineVotingApp.Models;
using System.Data.SqlClient;

namespace OnlineVotingApp.Pages.Admin
{
    public class ResultsModel : PageModel
    {
		public int VoteCount = 0;
		public string PartyWon = "";
		public int VotePrev = 0;
		public int WinningVote = 0;

		public List<VotingAppDetails> guestDetailList = new List<VotingAppDetails>();
		public void OnGet()
		{
			string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

			using (SqlConnection cnn = new SqlConnection(connectionString))
			{
				cnn.Open();
				string query = "SELECT PartyName, PartyCount FROM PartyDetails";
				using (SqlCommand cmd = new SqlCommand(query, cnn))
				{
					using (SqlDataReader dataReader = cmd.ExecuteReader())
					{
						while (dataReader.Read())
						{
							VotingAppDetails guestDetails = new VotingAppDetails();
							guestDetails.PartyName = dataReader.GetString(0);
							guestDetails.PartyCount = dataReader.GetInt32(1);
							if (Math.Max(VotePrev, guestDetails.PartyCount) > VotePrev)
							{
								PartyWon = guestDetails.PartyName;
								VotePrev = Math.Max(VotePrev, guestDetails.PartyCount);
								WinningVote = guestDetails.PartyCount;
							}


							VoteCount += guestDetails.PartyCount;

							guestDetailList.Add(guestDetails);
						}

					}
				}
			}
		}
	}
}
