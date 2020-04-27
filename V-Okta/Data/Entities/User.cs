using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace V_Okta.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }

        public List<Vote> Votes { get; set; }
    }
}
