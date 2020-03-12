using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class LookupProvince
    {
        public LookupProvince()
        {
            UserAddress = new HashSet<UserAddress>();
            UserCreditCard = new HashSet<UserCreditCard>();
        }

        public int Id { get; set; }
        public int? CountryId { get; set; }
        public string Province { get; set; }

        public virtual LookupCountry Country { get; set; }
        public virtual ICollection<UserAddress> UserAddress { get; set; }
        public virtual ICollection<UserCreditCard> UserCreditCard { get; set; }
    }
}
