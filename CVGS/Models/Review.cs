using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? GameId { get; set; }
        public string Username { get; set; }

        [Display(Name = "Review")]
        public string Review1 { get; set; }
        public int? Rating { get; set; }
        public DateTime Date { get; set; }
        public bool? ApprovedFlag { get; set; }

        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
    }
}
