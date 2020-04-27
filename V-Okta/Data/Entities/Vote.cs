using System.ComponentModel.DataAnnotations;

namespace V_Okta.Data.Entities
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Value { get; set; }

        public Movie Movie { get; set; }
        public User User { get; set; }
    }
}
