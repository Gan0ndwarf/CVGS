using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class WishListReportsModel
    {
        public Game Game { get; set; }

        [Display(Name = "Number of Users")]
        public int UserCount { get; set; }

    }
}