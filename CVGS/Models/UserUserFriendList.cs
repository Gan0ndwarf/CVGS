﻿using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class UserUserFriendList
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? FriendId { get; set; }

        public virtual User Friend { get; set; }
        public virtual User User { get; set; }
    }
}
