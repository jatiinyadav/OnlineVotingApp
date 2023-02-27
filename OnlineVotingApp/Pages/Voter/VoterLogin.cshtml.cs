using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineVotingApp.Models;
using System.Data.SqlClient;

namespace OnlineVotingApp.Pages.Voter
{
    public class VoterLoginModel : PageModel
    {
		public VotingAppDetails? guestDetails;
		
		public List<VotingAppDetails> partyNames = new List<VotingAppDetails>();
        public void OnGet()
        {
			try
			{
				string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

				using (SqlConnection cnn = new SqlConnection(connectionString))
				{
					cnn.Open();
					string query = "SELECT PartyName, SymbolName FROM PartyDetails";
					using (SqlCommand cmd = new SqlCommand(query,cnn))
					{
						using (SqlDataReader dataReader = cmd.ExecuteReader())
						{
							while (dataReader.Read())
							{
								guestDetails = new VotingAppDetails();
								guestDetails.PartyName = dataReader.GetString(0);
								guestDetails.PartySymbol = dataReader.GetString(1);

								partyNames.Add(guestDetails);
							}
						}
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
        }

        public void OnPost()
        {
			guestDetails = new VotingAppDetails();
			guestDetails.GuestName = Convert.ToString(Request.Form["guestName"]);
			guestDetails.GuestIDProof = Convert.ToString(Request.Form["guestAadhaar"]);
			guestDetails.PartyName = Convert.ToString(Request.Form["guestPartyChoose"]);
			guestDetails.PartySymbol = Convert.ToString(Request.Form["guestChosenPartySymbol"]);
			try
			{
				string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

				using (SqlConnection cnn = new SqlConnection(connectionString))
				{
					cnn.Open();
					string query = $"INSERT INTO GuestDetails VALUES ('{guestDetails.GuestName}', '{guestDetails.GuestIDProof}')" +
								   $"INSERT INTO VotingDetails VALUES ((SELECT MAX(GuestID) FROM GuestDetails), '{guestDetails.PartyName}', GETDATE());" +
								   $"UPDATE PartyDetails SET PartyCount = PartyCount + 1 WHERE PartyName = '{guestDetails.PartyName}'";
					using (SqlCommand cmd = new SqlCommand(query, cnn))
					{

						cmd.Parameters.AddWithValue("@guestName", guestDetails.GuestName);
						cmd.Parameters.AddWithValue("@guestAadhaar", guestDetails.GuestIDProof);
						cmd.Parameters.AddWithValue("@guestPartyChoose", guestDetails.PartyName);
						cmd.Parameters.AddWithValue("@guestChosenPartySymbol", guestDetails.PartySymbol);

						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{

			}

			Response.Redirect("/Voter/VotingPage");
            //if ()
            //{
                // Conditions
            //}
        }
    }

}
