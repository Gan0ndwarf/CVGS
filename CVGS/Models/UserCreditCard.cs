using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class UserCreditCard
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string CreditCardType { get; set; }
        public string CreditCardNumber { get; set; }
        public string SecurityCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string NameOnCard { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int? ProvinceId { get; set; }
        public int? CountryId { get; set; }
        public string PostalZipCode { get; set; }

        public virtual LookupCountry Country { get; set; }
        public virtual LookupProvince Province { get; set; }
        public virtual User User { get; set; }
    }
}
