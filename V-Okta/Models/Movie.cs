using System.Linq;

namespace V_Okta.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CurrentVotes { get; set; }

        public int UserVote { get; set; }

        public Movie(Data.Entities.Movie movie, int userId)
        {
            Id = movie.Id;
            Title = movie.Title;
            Description = movie.Description;
            CurrentVotes = movie.CurrentVotes;

            if (movie.Votes == null)
                return;

            var userVote = movie.Votes.Where(r => r.UserId.Equals(userId)).FirstOrDefault();

            if (userVote != null)
            {
                UserVote = userVote.Value;
            }
        }

        public Movie()
        {

        }
    }
}
