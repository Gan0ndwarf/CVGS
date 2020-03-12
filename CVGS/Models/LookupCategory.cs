using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class LookupCategory
    {
        public LookupCategory()
        {
            Game = new HashSet<Game>();
            UserCategoryFavouriteCategory = new HashSet<UserCategoryFavouriteCategory>();
        }

        public int Id { get; set; }
        public string Category { get; set; }

        public virtual ICollection<Game> Game { get; set; }
        public virtual ICollection<UserCategoryFavouriteCategory> UserCategoryFavouriteCategory { get; set; }
    }
}
