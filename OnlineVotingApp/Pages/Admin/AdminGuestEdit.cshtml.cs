using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineVotingApp.Models;
using System.Data.SqlClient;

namespace OnlineVotingApp.Pages.Admin
{
    public class AdminGuestEditModel : PageModel
    {

		public VotingAppDetails partyDetails = new VotingAppDetails();
		public List<VotingAppDetails> guestDetailList = new List<VotingAppDetails>();
		public List<VotingAppDetails> partyNamesList = new List<VotingAppDetails>();
		public VotingAppDetails? partyNames;

		[BindProperty]
		public string? MyProperty { get; set; }

		public void OnGet()
		{
			string ID = Convert.ToString(Request.Query["id"]);

			try
			{
				string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

				using (SqlConnection cnn = new SqlConnection(connectionString))
				{
					cnn.Open();
					string query = $"SELECT * FROM GuestDetails INNER JOIN VotingDetails ON GuestDetails.GuestID = VotingDetails.GuestID WHERE GuestDetails.GuestID = {ID}";

					using (SqlCommand cmd = new SqlCommand(query, cnn))
					{
						using (SqlDataReader dataReader = cmd.ExecuteReader())
						{
							while (dataReader.Read())
							{
								partyDetails.GuestName = dataReader.GetString(1);
								partyDetails.GuestIDProof = dataReader.GetString(2);
								partyDetails.PartyName = dataReader.GetString(5);

								guestDetailList.Add(partyDetails);
							}
						}
					}

					string query2 = $"SELECT PartyName, SymbolName FROM PartyDetails";

					using (SqlCommand cmd = new SqlCommand(query2, cnn))
					{
						using (SqlDataReader dataReader = cmd.ExecuteReader())
						{
							while (dataReader.Read())
							{
								partyNames = new VotingAppDetails();
								partyNames.PartyName = dataReader.GetString(0);
								partyNames.PartySymbol = dataReader.GetString(1);

								partyNamesList.Add(partyNames);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{

			}
		}

		public void OnPost()
        {
            MyProperty = Convert.ToString(Request.Form["partyPrevious"]);
            partyDetails.GuestName = Convert.ToString(Request.Form["guestName"]);
            partyDetails.GuestIDProof = Convert.ToString(Request.Form["guestAadhaar"]);
			partyDetails.PartyName = Convert.ToString(Request.Form["guestPartyChoose"]);

			string ID = Convert.ToString(Request.Query["id"]);

            try
            {
                string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cnn.Open();
                    string query = $"UPDATE GuestDetails SET GuestName = '{partyDetails.GuestName}', GuestIDProof = '{partyDetails.GuestIDProof}' WHERE GuestID = {ID}" +
                                   $"UPDATE VotingDetails SET PartyName = '{partyDetails.PartyName}' WHERE GuestID = {ID}" +
                                   $"UPDATE PartyDetails SET PartyCount = PartyCount - 1 WHERE PartyName = '{MyProperty}'" +
                                   $"UPDATE PartyDetails SET PartyCount = PartyCount + 1 WHERE PartyName = '{partyDetails.PartyName}'";
                    using (SqlCommand cmd = new SqlCommand(query, cnn))
                    {

                        cmd.Parameters.AddWithValue("@guestName", partyDetails.GuestName);
                        cmd.Parameters.AddWithValue("@guestAadhaar", partyDetails.GuestIDProof);
						cmd.Parameters.AddWithValue("@guestPartyChoose", partyDetails.PartyName);

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
