using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models
{
    public class PreferencesModel
    {
        public List<UserPlatformFavouritePlatform> FavouritePlatforms { get; set; }

        public List<UserCategoryFavouriteCategory> FavouriteGameCategories { get; set; }
    }
}
