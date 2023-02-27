using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineVotingApp.Models;
using System.Data.SqlClient;

namespace OnlineVotingApp.Pages.Admin
{
    public class AdminGuestCreateModel : PageModel
    {
        public VotingAppDetails guestDetails = new VotingAppDetails();
        public List<VotingAppDetails> partyNames = new List<VotingAppDetails>();
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
							guestDetails.PartyName = dataReader.GetString(1);
							guestDetails.PartySymbol = dataReader.GetString(2);

							partyNames.Add(guestDetails);
						}
					}
				}
			}
		}

        public void OnPost()
        {
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

            Response.Redirect("/Admin/AdminGuestUpdate");
        }
    }
}
