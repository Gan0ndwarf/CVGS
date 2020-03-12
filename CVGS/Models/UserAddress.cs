using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public partial class UserAddress
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Mailing")]
        public bool MailingFlag { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Line 1")]
        public string Address1 { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Line 2")]
        public string Address2 { get; set; }

        [Display(Name = "Province")]
        public int? ProvinceId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Country")]

        public int? CountryId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        public string PostalZipCode { get; set; }


        public LookupCountry Country { get; set; }

        public virtual LookupProvince Province { get; set; }

        public User User { get; set; }
    }
}
