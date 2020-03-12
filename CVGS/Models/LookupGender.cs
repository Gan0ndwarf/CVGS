using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class LookupGender
    {
        public LookupGender()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
