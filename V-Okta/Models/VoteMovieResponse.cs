namespace V_Okta.Models
{
    public class VoteMovieResponse
    {
        public bool Success { get; set; }
        public int CurrentVotes { get; set; }
        public int UserVote { get; set; }
    }
}
