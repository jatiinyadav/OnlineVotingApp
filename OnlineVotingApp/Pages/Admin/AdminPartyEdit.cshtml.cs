using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineVotingApp.Models;
using System.Data.SqlClient;

namespace OnlineVotingApp.Pages.Admin
{
    public class AdminPartyEditModel : PageModel
    {
		public VotingAppDetails partyDetails = new VotingAppDetails();
		public List<VotingAppDetails> guestDetailList = new List<VotingAppDetails>();

        [BindProperty]
        public string? previousParty { get; set; }

        public void OnGet()
        {
            string ID = Convert.ToString(Request.Query["id"]);

            try
            {
				string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

				using (SqlConnection cnn = new SqlConnection(connectionString))
				{
					cnn.Open();
					string query = $"SELECT * FROM PartyDetails WHERE PartyID = {ID}";
					using (SqlCommand cmd = new SqlCommand(query, cnn))
					{
						using (SqlDataReader dataReader = cmd.ExecuteReader())
						{
							while (dataReader.Read())
							{
								partyDetails.PartyName = dataReader.GetString(1);
								partyDetails.PartySymbol = dataReader.GetString(2);

								guestDetailList.Add(partyDetails);
							}
						}
					}
				}
			} catch (Exception ex)
			{

			}
        }

		public void OnPost()
		{
            previousParty = Convert.ToString(Request.Form["previousPartyName"]);
            partyDetails.PartyName = Convert.ToString(Request.Form["partyName"]);
            partyDetails.PartySymbol = Convert.ToString(Request.Form["partySymbolName"]);
            string ID = Convert.ToString(Request.Query["id"]);

            try
            {
                string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cnn.Open();
                    string query = $"UPDATE PartyDetails SET PartyName = '{partyDetails.PartyName}', SymbolName = '{partyDetails.PartySymbol}' WHERE PartyID = {ID}" +
                                   $"UPDATE VotingDetails SET PartyName = '{partyDetails.PartyName}' WHERE PartyName = '{previousParty}'";
                    using (SqlCommand cmd = new SqlCommand(query, cnn))
                    {

                        cmd.Parameters.AddWithValue("@partyName", partyDetails.PartyName);
                        cmd.Parameters.AddWithValue("@partySymbolName", partyDetails.PartySymbol);

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
