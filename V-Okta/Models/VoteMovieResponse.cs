using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V_Okta.Models
{
    public class VoteMovieResponse
    {
        public bool Success { get; set; }
        public int CurrentVotes { get; set; }
        public int UserVote { get; internal set; }
    }
}
