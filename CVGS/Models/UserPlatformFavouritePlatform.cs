using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class UserPlatformFavouritePlatform
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? PlatformId { get; set; }

        public virtual LookupPlatform Platform { get; set; }
        public virtual User User { get; set; }
    }
}
