using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace CVGSUITests
{
    public class CVGSUITests
    {
        private readonly IWebDriver driver;
        public CVGSUITests()
        {
            driver = new ChromeDriver(".");
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }


        [Fact]
        public void cvgs_Login()
        {
            //Tests that user can login

            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();
            string expected = "singithi";
            string actual = driver.FindElement(By.Id("username-span")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_Logout()
        {
            // Tests that user can logout
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("logout-button")).Click();

            string expected = "Log in";
            string actual = driver.FindElement(By.Id("login-button")).Text;
            Assert.Equal(expected, actual);
        }

        //[Fact]
        //public void cvgs_Register()
        //{

        //    driver.Navigate().GoToUrl("https://localhost:44396/Account/Register");
        //    driver.FindElement(By.Id("FirstName")).SendKeys("Test");
        //    driver.FindElement(By.Id("LastName")).SendKeys("Test");
        //    driver.FindElement(By.Id("GenderId")).SendKeys("M");
        //    driver.FindElement(By.Id("BirthDate")).SendKeys("01/10/1991");
        //    driver.FindElement(By.Id("Username")).SendKeys("testuser");
        //    driver.FindElement(By.Id("Email")).SendKeys("testuser@gmail.com");
        //    driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
        //    driver.FindElement(By.Id("ConfirmPassword")).SendKeys("Test1234!");
        //    driver.FindElement(By.Id("Register")).Click();

        //    string expected = "https://localhost:44396";
        //    string actual = driver.Url;

        //    Assert.Equal(expected, actual);
        //}

        [Fact]
        public void cvgs_Profile_GetIndex_WorksAspExpected()
        {
            // Tests that user can login and access Profile
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("profile-button")).Click();
            driver.FindElement(By.Id("profile-link")).Click();

            string expected = "My Profile";
            string actual = driver.FindElement(By.Id("profile-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_Address_GetIndex_WorksAsExpected()
        {
            // Tests that user can login and access Address
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("profile-button")).Click();
            driver.FindElement(By.Id("address-link")).Click();

            string expected = "Address";
            string actual = driver.FindElement(By.Id("address-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_Preferences_GetIndex_WorksAspExpected()
        {
            // Tests that user can login and access Preferences
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("profile-button")).Click();
            driver.FindElement(By.Id("preferences-link")).Click();

            string expected = "Preferences";
            string actual = driver.FindElement(By.Id("preferences-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_FriendsFamily_GetIndex_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("profile-button")).Click();
            driver.FindElement(By.Id("friendsfamily-link")).Click();

            string expected = "Friends and Family";
            string actual = driver.FindElement(By.Id("friendsfamily-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_FriendsFamilyWishList_GetIndex_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("profile-button")).Click();
            driver.FindElement(By.Id("friendsfamily-link")).Click();
            driver.FindElement(By.Id("friendsfamilywishlist-link")).Click();

            string expected = "Preferences";
            string actual = driver.FindElement(By.Id("preferences-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_SearchFriendsFamily_GetIndex_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("profile-button")).Click();
            driver.FindElement(By.Id("friendsfamily-link")).Click();
            driver.FindElement(By.Id("searchfriendsfamily-link")).Click();

            string expected = "Search Friends and Family";
            string actual = driver.FindElement(By.Id("searchfriendsfamily-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_WishList_GetIndex_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("profile-button")).Click();
            driver.FindElement(By.Id("wishlist-link")).Click();

            string expected = "Wish List";
            string actual = driver.FindElement(By.Id("wishlist-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_MyGames_GetIndex_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("profile-button")).Click();
            driver.FindElement(By.Id("mygames-link")).Click();

            string expected = "My Games";
            string actual = driver.FindElement(By.Id("mygames-header")).Text;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void cvgs_MyEvents_GetIndex_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("profile-button")).Click();
            driver.FindElement(By.Id("myevents-link")).Click();

            string expected = "My Events";
            string actual = driver.FindElement(By.Id("myevents-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageGames_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("managegames-link")).Click();

            string expected = "Manage Games";
            string actual = driver.FindElement(By.Id("managegames-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageGames_Create_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("managegames-link")).Click();
            driver.FindElement(By.Id("create-link")).Click();

            string expected = "Create";
            string actual = driver.FindElement(By.Id("creategames-header")).Text;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void cvgs_ManageGames_Edit_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("managegames-link")).Click();
            driver.FindElement(By.Id("editlink-1")).Click();

            string expected = "Edit";
            string actual = driver.FindElement(By.Id("editgames-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageGames_Details_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("managegames-link")).Click();
            driver.FindElement(By.Id("detailslink-1")).Click();

            string expected = "Details";
            string actual = driver.FindElement(By.Id("detailgames-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageGames_Delete_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("managegames-link")).Click();
            driver.FindElement(By.Id("deletelink-1")).Click();

            string expected = "Delete";
            string actual = driver.FindElement(By.Id("deletegames-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageEvents_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("manageevents-link")).Click();

            string expected = "Manage Events";
            string actual = driver.FindElement(By.Id("manageevents-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageEvents_Create_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("manageevents-link")).Click();
            driver.FindElement(By.Id("create-link")).Click();

            string expected = "Create Event";
            string actual = driver.FindElement(By.Id("createevents-header")).Text;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void cvgs_ManageEvents_Edit_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("manageevents-link")).Click();
            driver.FindElement(By.Id("editlink-1")).Click();

            string expected = "Edit";
            string actual = driver.FindElement(By.Id("editevents-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageEvents_Details_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("manageevents-link")).Click();
            driver.FindElement(By.Id("detailslink-1")).Click();

            string expected = "Event Details";
            string actual = driver.FindElement(By.Id("detailsevents-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageEvents_Delete_WorksAspExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("manageevents-link")).Click();
            driver.FindElement(By.Id("deletelink-1")).Click();

            string expected = "Delete Event";
            string actual = driver.FindElement(By.Id("deleteevents-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageReviews_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("managereviews-link")).Click();

            string expected = "Manage Reviews";
            string actual = driver.FindElement(By.Id("managereviews-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageReviews_ApproveReview_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("managereviews-link")).Click();
            driver.FindElement(By.Id("approvereview-7")).Click();

            string expected = "Manage Reviews";
            string actual = driver.FindElement(By.Id("managereviews-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_ManageReviews_RejectReview_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("managereviews-link")).Click();
            driver.FindElement(By.Id("rejectreview-8")).Click();

            string expected = "Manage Reviews";
            string actual = driver.FindElement(By.Id("managereviews-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_GameReports_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();
            
            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("gamereports-link")).Click();

            string expected = "Games Reports";
            string actual = driver.FindElement(By.Id("gamereports-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_GamesDetailsReport_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();
            
            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("gamedetailsreports-link")).Click();

            string expected = "Game Details Reports";
            string actual = driver.FindElement(By.Id("gamedetailsreports-header")).Text;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void cvgs_GameReviewsReport_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();
           
            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("gamereviewreports-link")).Click();

            string expected = "Game Review Reports";
            string actual = driver.FindElement(By.Id("gamereviewreports-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_MemberReports_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();
            
            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("memberreports-link")).Click();

            string expected = "Members Reports";
            string actual = driver.FindElement(By.Id("memberreports-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_MemberDetailsReports_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("memberdetailsreports-link")).Click();

            string expected = "Member Details Reports";
            string actual = driver.FindElement(By.Id("memberdetailsreports-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_OrderReports_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("salesreports-link")).Click();

            string expected = "Sales Reports";
            string actual = driver.FindElement(By.Id("salesreports-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_WishListReports_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("adminportal-button")).Click();
            driver.FindElement(By.Id("wishlistsreports-link")).Click();

            string expected = "Wish Lists Reports";
            string actual = driver.FindElement(By.Id("wishlistsreports-header")).Text;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void cvgs_GamesPage_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("gamespage-button")).Click();

            string expected = "Games Page";
            string actual = driver.FindElement(By.Id("gamespage-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_GamesDetailsPage__WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("gamespage-button")).Click();
            driver.FindElement(By.Id("gamelink-1")).Click();

            string expected = "Game Details";
            string actual = driver.FindElement(By.Id("gamedetails-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_RateGame_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("gamespage-button")).Click();
            driver.FindElement(By.Id("gamelink-1")).Click();
            driver.FindElement(By.Id("rategame-link")).Click();

            string expected = "Rate Game";
            string actual = driver.FindElement(By.Id("rategame-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_EditReview_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("gamespage-button")).Click();
            driver.FindElement(By.Id("gamelink-2")).Click();
            driver.FindElement(By.Id("editreview-link")).Click();

            string expected = "Edit Review";
            string actual = driver.FindElement(By.Id("editreview-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_DeleteReview_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("gamespage-button")).Click();
            driver.FindElement(By.Id("gamelink-2")).Click();
            driver.FindElement(By.Id("deletereview-link")).Click();

            string expected = "Game Details";
            string actual = driver.FindElement(By.Id("gamedetails-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_AddToWishList_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("gamespage-button")).Click();
            driver.FindElement(By.Id("gamelink-3")).Click();
            driver.FindElement(By.Id("addtowishlist-button")).Click();

            string expected = "Game Details";
            string actual = driver.FindElement(By.Id("gamedetails-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_AddToCart_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("gamespage-button")).Click();
            driver.FindElement(By.Id("gamelink-3")).Click();
            driver.FindElement(By.Id("addtocart-button")).Click();

            string expected = "Game Details";
            string actual = driver.FindElement(By.Id("gamedetails-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_Events_Index_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("eventspage-button")).Click();

            string expected = "Events";
            string actual = driver.FindElement(By.Id("eventspage-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_Events_Register_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.FindElement(By.Id("account-dropdown")).Click();
            driver.FindElement(By.Id("eventspage-button")).Click();
            driver.FindElement(By.Id("register-button")).Click();

            string expected = "Events";
            string actual = driver.FindElement(By.Id("eventspage-header")).Text;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void cvgs_CreditCard_Add_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.Navigate().GoToUrl("https://localhost:44396/Member/CreditCardsCreate");
            driver.FindElement(By.Id("cardnumber")).SendKeys("4875111111111111");
            driver.FindElement(By.Id("code")).SendKeys("123");
            driver.FindElement(By.Id("ExpDate")).SendKeys("July");
            driver.FindElement(By.Id("ExpDate")).SendKeys(Keys.Tab);
            driver.FindElement(By.Id("ExpDate")).SendKeys("2020");
            driver.FindElement(By.Id("name")).SendKeys("Liam");
            driver.FindElement(By.Id("address")).SendKeys("12 testing street");
            driver.FindElement(By.Id("city")).SendKeys("guelph");
            driver.FindElement(By.Id("province")).SendKeys("O");
            driver.FindElement(By.Id("country")).SendKeys("C");
            driver.FindElement(By.Id("zip")).SendKeys("N1G1D2");
            driver.FindElement(By.Id("submit")).Click();
            
            string expected = "Index";
            string actualTitle = driver.getTitle();
            //string actual = driver.FindElement(By.Id("eventspage-header")).Text;
            Assert.Equal(expected, actualTitle);
        }
        [Fact]
        public void cvgs_CreditCard_Delete_WorksAsExpected()
        {
            //since card is added in previous test it should delete fine
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.Navigate().GoToUrl("https://localhost:44396/Member/CreditCardsIndex");
            driver.FindElement(By.Id("deletecard")).Click();
            driver.FindElement(By.Id("deleteConfirm")).Click();

            string expected = "Index";
            string actualTitle = driver.getTitle();
            //string actual = driver.FindElement(By.Id("eventspage-header")).Text;
            Assert.Equal(expected, actualTitle);
        }
        [Fact]
        public void cvgs_PurchaseWithCard_WorksAsExpected()
        {
            // Tests that user can login and access Friends and Family WishList
            driver.Navigate().GoToUrl("https://localhost:44396/Account/Login");
            driver.FindElement(By.Id("Username")).SendKeys("singithi");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("Login")).Click();

            driver.Navigate().GoToUrl("https://localhost:44396/Member/CreditCardsCreate");
            driver.FindElement(By.Id("cardnumber")).SendKeys("4875111111111111");
            driver.FindElement(By.Id("code")).SendKeys("123");
            driver.FindElement(By.Id("ExpDate")).SendKeys("July");
            driver.FindElement(By.Id("ExpDate")).SendKeys(Keys.Tab);
            driver.FindElement(By.Id("ExpDate")).SendKeys("2020");
            driver.FindElement(By.Id("name")).SendKeys("Liam");
            driver.FindElement(By.Id("address")).SendKeys("12 testing street");
            driver.FindElement(By.Id("city")).SendKeys("guelph");
            driver.FindElement(By.Id("province")).SendKeys("O");
            driver.FindElement(By.Id("country")).SendKeys("C");
            driver.FindElement(By.Id("zip")).SendKeys("N1G1D2");
            driver.FindElement(By.Id("submit")).Click();
            driver.Navigate().GoToUrl("https://localhost:44396/Game");
            driver.FindElement(By.Id("gamelink-1")).Click();
            driver.FindElement(By.Id("addtowishlist-button")).Click();
            driver.Navigate().GoToUrl("https://localhost:44396/Cart");
            driver.FindElement(By.Id("proceed-to-billing")).Click();
            driver.FindElement(By.Id("submit")).Click();
            string expected = "Order Confirmation";
            string actualTitle = driver.getTitle();
            //string actual = driver.FindElement(By.Id("eventspage-header")).Text;
            Assert.Equal(expected, actualTitle);
        }
    }
}
