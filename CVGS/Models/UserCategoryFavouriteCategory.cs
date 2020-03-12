using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class UserCategoryFavouriteCategory
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }

        public virtual LookupCategory Category { get; set; }
        public virtual User User { get; set; }
    }
}
