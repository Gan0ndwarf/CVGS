using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVGS.Models
{
    public class AddressModel
    {
        [Display(Name = "Home Address")]
        public UserAddress HomeAddress { get; set; }

        [Display(Name = "Mailing Address")]
        public UserAddress MailingAddress { get; set; }

    }
}