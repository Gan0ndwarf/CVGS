using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Models.EmployeeViewModels
{
    public class PaymentViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Selected Payment Method")]
        public string SelectedPaymentMethod { get; set; }

       }
}
