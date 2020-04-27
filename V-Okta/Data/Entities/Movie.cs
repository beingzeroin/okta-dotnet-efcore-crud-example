using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace V_Okta.Data.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CurrentVotes { get; set; }

        public List<Vote> Votes { get; set; }

    }
}
