using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CVGS.Models
{
    public partial class CVGSContext : DbContext
    {
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<LookupCategory> LookupCategory { get; set; }
        public virtual DbSet<LookupCountry> LookupCountry { get; set; }
        public virtual DbSet<LookupGender> LookupGender { get; set; }
        public virtual DbSet<LookupPlatform> LookupPlatform { get; set; }
        public virtual DbSet<LookupProvince> LookupProvince { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAddress> UserAddress { get; set; }
        public virtual DbSet<UserCategoryFavouriteCategory> UserCategoryFavouriteCategory { get; set; }
        public virtual DbSet<UserCreditCard> UserCreditCard { get; set; }
        public virtual DbSet<UserEvent> UserEvent { get; set; }
        public virtual DbSet<UserGameWishList> UserGameWishList { get; set; }
        public virtual DbSet<UserPlatformFavouritePlatform> UserPlatformFavouritePlatform { get; set; }
        public virtual DbSet<UserUserFriendList> UserUserFriendList { get; set; }

        public CVGSContext()
        {
        }

        public CVGSContext(DbContextOptions<CVGSContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("time(4)");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Developer)
                    .HasColumnName("developer")
                    .HasMaxLength(50);

                entity.Property(e => e.ImgLocation)
                    .HasColumnName("img_location")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.PlatformId).HasColumnName("platform_id");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(19, 2)");

                entity.Property(e => e.Rating)
                    .HasColumnName("rating")
                    .HasColumnType("decimal(19, 1)");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("release_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Game_LookupCategory");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.PlatformId)
                    .HasConstraintName("FK_Game_LookupPlatform");
            });

            modelBuilder.Entity<LookupCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LookupCountry>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LookupGender>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LookupPlatform>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Platform)
                    .IsRequired()
                    .HasColumnName("platform")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LookupProvince>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasColumnName("province")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.LookupProvince)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_LookupProvince_LookupCountry");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Order_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Order_User");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApprovedFlag).HasColumnName("approved_flag");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Review1).HasColumnName("review");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Review_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Review_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.EmailFlag).HasColumnName("email_flag");

                entity.Property(e => e.EmployeeFlag).HasColumnName("employee_flag");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.GenderId).HasColumnName("gender_id");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHashed)
                    .IsRequired()
                    .HasColumnName("password_hashed")
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(10);

                entity.Property(e => e.PrivateFlag).HasColumnName("private_flag");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_User_LookupGender");
            });

            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address1)
                    .HasColumnName("address1")
                    .HasMaxLength(50);

                entity.Property(e => e.Address2)
                    .HasColumnName("address2")
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.MailingFlag).HasColumnName("mailing_flag");

                entity.Property(e => e.PostalZipCode)
                    .HasColumnName("postal_zip_code")
                    .HasMaxLength(16);

                entity.Property(e => e.ProvinceId).HasColumnName("province_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.UserAddress)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_UserAddress_LookupCountry");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.UserAddress)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_UserAddress_LookupProvince");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAddress)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserAddress_User");
            });

            modelBuilder.Entity<UserCategoryFavouriteCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.UserCategoryFavouriteCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserCategoryFavouriteCategory_LookupCategory");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserCategoryFavouriteCategory)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserCategoryFavouriteCategory_User");
            });

            modelBuilder.Entity<UserCreditCard>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address1)
                    .HasColumnName("address1")
                    .HasMaxLength(50);

                entity.Property(e => e.Address2)
                    .HasColumnName("address2")
                    .HasMaxLength(50);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50);

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CreditCardNumber)
                    .IsRequired()
                    .HasColumnName("credit_card_number")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.CreditCardType)
                    .IsRequired()
                    .HasColumnName("credit_card_type")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ExpirationDate)
                    .HasColumnName("expiration_date")
                    .HasColumnType("date");

                entity.Property(e => e.NameOnCard)
                    .HasColumnName("name_on_card")
                    .HasMaxLength(50);

                entity.Property(e => e.PostalZipCode)
                    .HasColumnName("postal_zip_code")
                    .HasMaxLength(16);

                entity.Property(e => e.ProvinceId).HasColumnName("province_id");

                entity.Property(e => e.SecurityCode)
                    .IsRequired()
                    .HasColumnName("security_code")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.UserCreditCard)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserCreditCard_LookupCountry");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.UserCreditCard)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_UserCreditCard_LookupProvince1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserCreditCard)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserCreditCard_User");
            });

            modelBuilder.Entity<UserEvent>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.UserEvent)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserEvent_Event");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserEvent)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserEvent_User");
            });

            modelBuilder.Entity<UserGameWishList>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.UserGameWishList)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserGameWishList_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGameWishList)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserGameWishList_User");
            });

            modelBuilder.Entity<UserPlatformFavouritePlatform>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PlatformId).HasColumnName("platform_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.UserPlatformFavouritePlatform)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserPlatformFavouritePlatform_LookupPlatform");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPlatformFavouritePlatform)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserPlatformFavouritePlatform_User");
            });

            modelBuilder.Entity<UserUserFriendList>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FriendId).HasColumnName("friend_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Friend)
                    .WithMany(p => p.UserUserFriendListFriend)
                    .HasForeignKey(d => d.FriendId)
                    .HasConstraintName("FK_UserUserFriendList_User1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserUserFriendListUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserUserFriendList_User");
            });
        }
    }
}
