using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class LookupPlatform
    {
        public LookupPlatform()
        {
            Game = new HashSet<Game>();
            UserPlatformFavouritePlatform = new HashSet<UserPlatformFavouritePlatform>();
        }

        public int Id { get; set; }
        public string Platform { get; set; }

        public virtual ICollection<Game> Game { get; set; }
        public virtual ICollection<UserPlatformFavouritePlatform> UserPlatformFavouritePlatform { get; set; }
    }
}
