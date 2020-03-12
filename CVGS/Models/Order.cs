using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public int? GameId { get; set; }

        [Display(Name = "Date Purchased")]
        public DateTime? Date { get; set; }

        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
    }
}
