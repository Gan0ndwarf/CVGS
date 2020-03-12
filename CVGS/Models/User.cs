using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public partial class User
    {
        public User()
        {
            Order = new HashSet<Order>();
            Review = new HashSet<Review>();
            UserCategoryFavouriteCategory = new HashSet<UserCategoryFavouriteCategory>();
            UserEvent = new HashSet<UserEvent>();
            UserGameWishList = new HashSet<UserGameWishList>();
            UserPlatformFavouritePlatform = new HashSet<UserPlatformFavouritePlatform>();
            UserUserFriendListFriend = new HashSet<UserUserFriendList>();
            UserUserFriendListUser = new HashSet<UserUserFriendList>();
            UserCreditCard = new HashSet<UserCreditCard>();
        }

        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "Employee Flag")]
        public bool EmployeeFlag { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Private Flag")]
        public bool PrivateFlag { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Password")]
        public string PasswordHashed { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Gender")]
        public int? GenderId { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? Dob { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Email Flag")]
        public bool EmailFlag { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        public Guid? Salt { get; set; }


        public LookupGender Gender { get; set; }
        public ICollection<Order> Order { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<UserCategoryFavouriteCategory> UserCategoryFavouriteCategory { get; set; }
        public ICollection<UserCreditCard> UserCreditCard { get; set; }
        public ICollection<UserAddress> UserAddress { get; set; }
        public ICollection<UserEvent> UserEvent { get; set; }
        public ICollection<UserGameWishList> UserGameWishList { get; set; }
        public ICollection<UserPlatformFavouritePlatform> UserPlatformFavouritePlatform { get; set; }
        public ICollection<UserUserFriendList> UserUserFriendListFriend { get; set; }
        public ICollection<UserUserFriendList> UserUserFriendListUser { get; set; }
    }
}
