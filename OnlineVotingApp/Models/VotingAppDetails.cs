using System.ComponentModel.DataAnnotations;

namespace OnlineVotingApp.Models
{
    public class VotingAppDetails
    {
        [Key]
        public int GuestID { get; set; }

        public string? GuestName { get; set; }

        public string? GuestIDProof { get; set; }

		public string? PartyName { get; set; }

		public DateTime DateDetails { get; set; }

		public int PartyID { get; set; }

		public string? PartySymbol { get; set; }

		public int PartyCount { get; set; }
	}
}
