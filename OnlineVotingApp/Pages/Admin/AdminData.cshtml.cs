using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineVotingApp.Models;
using System.Data;
using System.Data.SqlClient;

namespace OnlineVotingApp.Pages.Admin
{
	public class AdminDataModel : PageModel
	{
		public string errorMessage = "";
		public VotingAppDetails? guestDetails;
		public List<VotingAppDetails> guestDetailList = new List<VotingAppDetails>();

		[BindProperty]
		public string? SearchGuestName { get; set; }

		public void OnGet(string GuestName = "")

		{
			try
			{
				string connectionString = "Data Source=SP13884\\SQLEXPRESS;Initial Catalog=assessmentDB;Integrated Security=True";

				DataSet ds = new DataSet();

				using (SqlConnection cnn = new SqlConnection(connectionString))
				{
					cnn.Open();

					string query = "";

					if (GuestName.Length > 0)
					{
						query = $"SELECT GuestDetails.*, VotingDetails.VoteID, PartyName, VoteDate FROM GuestDetails INNER JOIN VotingDetails ON GuestDetails.GuestID = VotingDetails.GuestID WHERE GuestName LIKE '{SearchGuestName}%'";
						SqlDataAdapter da = new SqlDataAdapter(query, cnn);
						da.Fill(ds);

						if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
						{
							errorMessage = "Data fetched successfully";
						} else
						{
							errorMessage = $"Couldn't find the Guest Name: {GuestName}";
							return;
						}
					}
					else
					{
						query = "SELECT GuestDetails.*, VotingDetails.VoteID, PartyName, VoteDate FROM GuestDetails INNER JOIN VotingDetails ON GuestDetails.GuestID = VotingDetails.GuestID ORDER BY GuestID ASC";

					}
					using (SqlCommand cmd = new SqlCommand(query, cnn))
					{
						using (SqlDataReader dataReader = cmd.ExecuteReader())
						{
							while (dataReader.Read())
							{
								guestDetails = new VotingAppDetails();
								guestDetails.GuestID = dataReader.GetInt32(0);
								guestDetails.GuestName = dataReader.GetString(1);
								guestDetails.GuestIDProof = dataReader.GetString(2);
								guestDetails.PartyName = dataReader.GetString(4);
								guestDetails.DateDetails = dataReader.GetDateTime(5);

								guestDetailList.Add(guestDetails);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = "Coudn't find the guest";
				throw;
			}
			


		}

		public void OnPost()
		{
			SearchGuestName = Convert.ToString(Request.Form["searchGuestName"]);
			OnGet(SearchGuestName);
		}


	}

}
