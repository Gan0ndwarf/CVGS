using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public partial class Game
    {
        public Game()
        {
            Review = new HashSet<Review>();
            Order = new HashSet<Order>();
            UserGameWishList = new HashSet<UserGameWishList>();
        }

        [Display(Name = "Game ID")]
        public int GameId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string ImgLocation { get; set; }

        [Display(Name = "Platform")]
        public int PlatformId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime? ReleaseDate { get; set; }
        public string Description { get; set; }
        public decimal? Rating { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string Developer { get; set; }

        public LookupCategory Category { get; set; }
        public LookupPlatform Platform { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<Order> Order { get; set; }
        public ICollection<UserGameWishList> UserGameWishList { get; set; }
    }
}
