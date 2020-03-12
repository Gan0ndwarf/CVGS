using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class GameModel
    {
        public Game Game { get; set; }

        [Display(Name = "Overall Rating")]
        public decimal OverallRating { get; set; }

        [Display(Name = "User Reviews")]
        public Review UserReview { get; set; }

        public List<Review> Reviews { get; set; }

        public bool OnWishList { get; set; }

        public bool InCart { get; set; }

        public bool Purchased { get; set; }

    }
}