using System;
using System.Collections.Generic;

namespace CVGS.Models
{
    public partial class LookupCountry
    {
        public LookupCountry()
        {
            LookupProvince = new HashSet<LookupProvince>();
            UserAddress = new HashSet<UserAddress>();
            UserCreditCard = new HashSet<UserCreditCard>();
        }

        public int Id { get; set; }
        public string Country { get; set; }

        public virtual ICollection<LookupProvince> LookupProvince { get; set; }
        public virtual ICollection<UserAddress> UserAddress { get; set; }
        public virtual ICollection<UserCreditCard> UserCreditCard { get; set; }
    }
}
