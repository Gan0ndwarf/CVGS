using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class UserGameWishList
    {
        public int Id { get; set; }
        public int? GameId { get; set; }
        public int? UserId { get; set; }

        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
    }
}
