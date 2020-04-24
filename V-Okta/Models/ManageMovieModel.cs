using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V_Okta.Models
{
    public class ManageMovieModel
    {
        public int RequestedId { get; set; }
        public bool NewMovie { get; set; }
        public bool MovieFound { get { return Movie != null || NewMovie; } }
        public Movie Movie { get; set; }
    }
}
