using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models.EmployeeViewModels
{
    public class WishListReportsViewModel
    {

        [DataType(DataType.Text)]
        [Display(Name = "Line 1")]
        public string Line1 { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Line 2")]
        public string Line2 { get; set; }


    }
}
