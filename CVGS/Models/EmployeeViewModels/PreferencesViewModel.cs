using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models.EmployeeViewModels
{
    public class PreferencesViewModel
    {
        public List<UserPlatformFavouritePlatform> FavouritePlatforms { get; set; }

        public List<UserCategoryFavouriteCategory> FavouriteGameCategories { get; set; }

    }
}
