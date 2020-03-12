using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public partial class PayBillModel
    {

        public List<CreditCardModel> UserCards { get; set; }

        public CreditCardModel AddCard { get; set; }
    }
}
