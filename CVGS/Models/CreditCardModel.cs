using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public partial class CreditCardModel
    {

        public int? Id { get; set; }
        public int? UserId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Card Type")]
        public string CreditCardType { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Card Number")]
        public string CreditCardNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(3)]
        [MinLength(3)]
        [Display(Name = "Security Code")]
        public string SecurityCode { get; set; }

        [Required]
        [Display(Name = "Month")]
        public int Month { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name (As Appears on Card)")]
        public string NameOnCard { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Line 1")]
        public string Address1 { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Line 2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Province")]
        public int? ProvinceId { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        [MaxLength(16)]
        public string PostalZipCode { get; set; }

        public virtual LookupCountry User { get; set; }

        public virtual LookupCountry Country { get; set; }
        public virtual LookupProvince Province { get; set; }
    }
}
