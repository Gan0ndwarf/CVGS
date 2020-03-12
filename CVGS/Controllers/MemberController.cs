using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS.Models;
using CVGS.Models.EmployeeViewModels;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CVGS.Controllers
{
    public class MemberController : Controller
    {
        private readonly CVGSContext _context;

        public MemberController(CVGSContext context)
        {
            _context = context;
        }
        Regex visa = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
        Regex mastercard = new Regex("^5[1-5][0-9]{14}$");
        Regex express = new Regex("^3[47][0-9]{13}$");
        public IActionResult Index(int? userId)
        {

            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));
            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }


            return View();
        }

        public async Task<IActionResult> Profile(int? userId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));
            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var emp = await _context.User
                          .Include(a => a.Gender)
                          .SingleOrDefaultAsync(m => m.UserId == userId);

            ViewData["GenderList"] = new SelectList(_context.LookupGender.OrderBy(p => p.Id), "Id", "Gender", emp.GenderId);

            if (emp == null)
            {
                ModelState.AddModelError("", "The employee you asked for does not exist.");
            }

            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(int? id, [Bind("UserId,EmployeeFlag,PrivateFlag,Username,PasswordHashed, FirstName, LastName, GenderId, Dob, Email, Phone, EmailFlag, Salt")]User user)
        {

            if (id != user.UserId)
            {
                return NotFound();
            }

            AspNetUsers aspUser = await _context.AspNetUsers.SingleOrDefaultAsync(a => a.Id == id.ToString());

            aspUser.UserName = user.Username;
            aspUser.NormalizedUserName = user.Username.ToUpper();
            aspUser.Email = user.Email;
            aspUser.NormalizedEmail = user.Email.ToUpper();


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    _context.Update(aspUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        public async Task<IActionResult> Address(int? userId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));

            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            UserAddress homeAddress = await _context.UserAddress
                                .Include(a => a.Country)
                                .SingleOrDefaultAsync(m => m.UserId == userId && m.MailingFlag == false);

            UserAddress mailingAddress = await _context.UserAddress
                                .Include(a => a.Country)
                                .SingleOrDefaultAsync(m => m.UserId == userId && m.MailingFlag == true);

            ViewData["ProvinceList"] = new SelectList(_context.LookupProvince.OrderBy(p => p.Id), "Id", "Province");
            ViewData["CountryList"] = new SelectList(_context.LookupCountry.OrderBy(p => p.Id), "Id", "Country");

            if (homeAddress == null)
            {
                homeAddress = new UserAddress();
                homeAddress.Address1 = null;
                homeAddress.Address2 = null;
                homeAddress.City = null;
                homeAddress.ProvinceId = null;
                homeAddress.CountryId = null;
                homeAddress.PostalZipCode = null;
            }
            if (mailingAddress == null)
            {
                mailingAddress = new UserAddress();
                mailingAddress.Address1 = null;
                mailingAddress.Address2 = null;
                mailingAddress.City = null;
                mailingAddress.ProvinceId = null;
                mailingAddress.CountryId = null;
                mailingAddress.PostalZipCode = null;
            }

            AddressModel addresses = new AddressModel();
            addresses.HomeAddress = homeAddress;
            addresses.MailingAddress = mailingAddress;

            return View(addresses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Address(AddressModel model)
        {

            var homeAddress = await _context.UserAddress
                                .Include(a => a.Country)
                                .SingleOrDefaultAsync(m => m.Id == model.HomeAddress.Id);

            var mailingAddress = await _context.UserAddress
                                .Include(a => a.Country)
                                .SingleOrDefaultAsync(m => m.Id == model.MailingAddress.Id);

            if (ModelState.IsValid)
            {
                try
                {
                    // if home address doesn't exist create record
                    if (homeAddress == null)
                    {
                        _context.Add(model.HomeAddress);
                    }
                    // if home address exists, update record
                    if (homeAddress != null)
                    {
                        homeAddress.Id = model.HomeAddress.Id;
                        homeAddress.UserId = model.HomeAddress.UserId;
                        homeAddress.Address1 = model.HomeAddress.Address1;
                        homeAddress.Address2 = model.HomeAddress.Address2;
                        homeAddress.City = model.HomeAddress.City;
                        homeAddress.ProvinceId = model.HomeAddress.ProvinceId;
                        homeAddress.CountryId = model.HomeAddress.CountryId;
                        homeAddress.PostalZipCode = model.HomeAddress.PostalZipCode;
                        _context.Update(homeAddress);
                    }
                    // if mailing address doesn't exist create record
                    if (mailingAddress == null)
                    {
                        _context.Add(model.MailingAddress);
                    }
                    // if mailing address exists, update record
                    if (mailingAddress != null)
                    {
                        mailingAddress.Id = model.MailingAddress.Id;
                        mailingAddress.UserId = model.MailingAddress.UserId;
                        mailingAddress.Address1 = model.MailingAddress.Address1;
                        mailingAddress.Address2 = model.MailingAddress.Address2;
                        mailingAddress.City = model.MailingAddress.City;
                        mailingAddress.ProvinceId = model.MailingAddress.ProvinceId;
                        mailingAddress.CountryId = model.MailingAddress.CountryId;
                        mailingAddress.PostalZipCode = model.MailingAddress.PostalZipCode;

                        _context.Update(mailingAddress);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CountryList"] = new SelectList(_context.LookupCountry.OrderBy(p => p.Id), "Id", "Country");

            return View(model);
        }

        public async Task<IActionResult> Preferences(int? userId)
        {

            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));
            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var favouritePlatforms = await _context.UserPlatformFavouritePlatform
                            .Include(t => t.Platform)
                            .Include(t => t.User)
                            .Where(t => t.UserId == userId)
                            .ToListAsync();


            var favouriteCategories = await _context.UserCategoryFavouriteCategory
                           .Include(t => t.Category)
                           .Include(t => t.User)
                           .Where(t => t.UserId == userId)
                           .ToListAsync();

            PreferencesModel preferences = new PreferencesModel();
            preferences.FavouritePlatforms = favouritePlatforms;
            preferences.FavouriteGameCategories = favouriteCategories;

            return View(preferences);
        }

        public IActionResult AddPlatformPreference(int? userId)
        {
            ViewData["PlatformList"] = new SelectList(_context.LookupPlatform.OrderBy(p => p.Platform), "Id", "Platform");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlatformPreference([Bind("UserId, PlatformId")]UserPlatformFavouritePlatform platformPreference)
        {

            var platform = await _context.LookupPlatform.SingleOrDefaultAsync(t => t.Id == platformPreference.PlatformId);
            var user = await _context.User.SingleOrDefaultAsync(t => t.UserId == platformPreference.UserId);

            platformPreference.Platform = platform;
            platformPreference.User = user;


            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserPlatformFavouritePlatform.Add(platformPreference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return RedirectToAction(nameof(Preferences));
        }

        public IActionResult AddCategoryPreference(int? userId)
        {
            ViewData["CategoryList"] = new SelectList(_context.LookupCategory.OrderBy(c => c.Category), "Id", "Category");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategoryPreference([Bind("UserId, CategoryId")]UserCategoryFavouriteCategory categoryPreference)
        {

            var category = await _context.LookupCategory.SingleOrDefaultAsync(t => t.Id == categoryPreference.CategoryId);
            var user = await _context.User.SingleOrDefaultAsync(t => t.UserId == categoryPreference.UserId);

            categoryPreference.Category = category;
            categoryPreference.User = user;


            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserCategoryFavouriteCategory.Add(categoryPreference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return RedirectToAction(nameof(Preferences));
        }


        public async Task<IActionResult> DeletePlatformPreference(int? userId, int? platformId)
        {

            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));
            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var favouritePlatform = await _context.UserPlatformFavouritePlatform
                            .SingleOrDefaultAsync(t => t.PlatformId == platformId && t.UserId == userId);


            if (favouritePlatform == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserPlatformFavouritePlatform.Remove(favouritePlatform);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return RedirectToAction(nameof(Preferences));
        }

        public async Task<IActionResult> DeleteCategoryPreference(int? userId, int? categoryId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));
            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var favouriteCategory = await _context.UserCategoryFavouriteCategory
                            .SingleOrDefaultAsync(t => t.CategoryId == categoryId && t.UserId == userId);


            if (favouriteCategory == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserCategoryFavouriteCategory.Remove(favouriteCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return RedirectToAction(nameof(Preferences));
        }


        public async Task<IActionResult> PaymentInformation(int? userId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));
            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var employee = await _context.User
                .SingleOrDefaultAsync(m => m.UserId == userId);

            if (employee == null)
            {
                ModelState.AddModelError("", "The employee you asked for does not exist.");
            }

            return View(employee);
        }

        public async Task<IActionResult> FriendsFamily(int? userId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));

            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var friendsFamily = await _context.UserUserFriendList
                                    .Include(a => a.Friend)
                                    .Where(t => t.UserId == userId)
                                    .ToListAsync();

            return View(friendsFamily);
        }


        public IActionResult SearchFriendsFamily(string searchString, int? userId)
        {
            List<User> friends = new List<User>();
            var friendIds = _context.UserUserFriendList.Where(t => t.UserId == userId).Select(t => t.FriendId).ToList();

            var users = _context.User.Where(t => t.UserId != userId);
            for (int i = 0; i < friendIds.Count; i++)
            {
                var friend = _context.User.SingleOrDefault(t => t.UserId == friendIds[i]);
                friends.Add(friend);
            }

            users = users.Except(friends);


            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                users = users.Where(g => g.FirstName.ToLower().Contains(searchString) || g.LastName.ToLower().Contains(searchString) || g.Username.ToLower().Contains(searchString));
            }

            return View(users.ToList());
        }


        public IActionResult AddFriendsFamily(int userId, int friendId)
        {
            UserUserFriendList model = new UserUserFriendList();

            User user = _context.User.SingleOrDefault(t => t.UserId == userId);
            User friend = _context.User.SingleOrDefault(t => t.UserId == friendId);

            model.UserId = user.UserId;
            model.FriendId = friend.UserId;
            model.User = user;
            model.Friend = friend;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFriendsFamily([Bind("UserId,FriendId")]UserUserFriendList selectedFriend)
        {

            if (ModelState.IsValid)
            {
                _context.Add(selectedFriend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FriendsFamily));
            }
            return View(selectedFriend);
        }

        public async Task<IActionResult> DeleteFriendFamily(int? userId, int? friendId)
        {
            if (userId == null || friendId == null)
            {
                return NotFound();
            }

            var user = await _context.UserUserFriendList
                            .SingleOrDefaultAsync(t => t.UserId == userId && t.FriendId == friendId);

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserUserFriendList.Remove(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return RedirectToAction(nameof(FriendsFamily));
        }

        public async Task<IActionResult> WishList(int? userId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));
            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var wishList = _context.UserGameWishList
                .Include(t => t.Game)
                .Where(t => t.UserId == userId);

            return View(await wishList.ToListAsync());
        }


        public async Task<IActionResult> DeleteWishListItem(int? gameId, int? userId)
        {
            if (gameId == null || userId == null)
            {
                return NotFound();
            }

            var game = await _context.UserGameWishList
                          .Include(t => t.Game)
                          .ThenInclude(t => t.Platform)
                          .Include(t => t.Game)
                          .ThenInclude(t => t.Category)
                          .SingleOrDefaultAsync(m => m.GameId == gameId && m.UserId == userId);

            if (game == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.UserGameWishList.Remove(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return RedirectToAction(nameof(WishList));
        }



        public async Task<IActionResult> FriendsWishList(int? friendId)
        {

            var friend = await _context.User
              .Include(a => a.Gender)
              .SingleOrDefaultAsync(m => m.UserId == friendId);

            if (friend == null)
            {
                return NotFound();
            }

            ViewData["Title"] = $"{friend.FirstName} {friend.LastName}'s Wish List";

            var friendsWishlist = _context.UserGameWishList
                .Include(t => t.Game)
                .Include(t => t.User)
                .Where(t => t.UserId == friendId && t.User.PrivateFlag == true);

            return View(await friendsWishlist.ToListAsync());
        }

        public async Task<IActionResult> MyGames(int? userId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));

            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var myOrders = _context.Order
                .Include(t => t.User)
                .Include(t => t.Game)
                .Where(t => t.UserId == userId);

            return View(await myOrders.ToListAsync());
        }

        public async Task<IActionResult> MyGameDetails(int? orderId)
        {

            var myOrder = _context.Order
                .Include(t => t.User)
                .Include(t => t.Game)
                .ThenInclude(t => t.Category)
                .Include(t => t.Game)
                .ThenInclude(t => t.Platform)
                .SingleOrDefaultAsync(t => t.OrderId == orderId);

            return View(await myOrder);
        }

        public async Task<IActionResult> MyEvents(int? userId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));

            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }

            var events = _context.UserEvent
                .Include(t => t.Event)
                .Where(t => t.UserId == userId);

            return View(await events.ToListAsync());
        }

        public async Task<IActionResult> EventDetails(int? userId, int? eventId)
        {
            if (userId == null || eventId == null)
            {
                return NotFound();
            }

            var selectedEvent = await _context.UserEvent
                .Include(t => t.Event)
                .SingleOrDefaultAsync(t => t.UserId == userId && t.EventId == eventId);

            return View(selectedEvent);
        }

        public async Task<IActionResult> DeleteEvent(int? userId, int? eventId)
        {
            if (userId == null || eventId == null)
            {
                return NotFound();
            }

            var selectedEvent = await _context.UserEvent
                .Include(t => t.Event)
                .SingleOrDefaultAsync(t => t.UserId == userId && t.EventId == eventId);

            if (selectedEvent == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserEvent.Remove(selectedEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            return RedirectToAction(nameof(MyEvents));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // GET: UserCreditCards
        public async Task<IActionResult> CreditCardsIndex(int? userId)
        {
            if (userId != null)
            {
                HttpContext.Session.SetString(nameof(userId), userId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(userId)) != null)
            {
                userId = Convert.ToInt32(HttpContext.Session.GetString(nameof(userId)));

            }
            else
            {
                TempData["message"] = "Please log in.";
                return Redirect("/Home/Index");
            }
            var cVGSContext = _context.UserCreditCard.Include(u => u.Country).Include(u => u.Province).Include(u => u.User).Where(u => u.UserId == userId);
            return View(await cVGSContext.ToListAsync());
        }

        // GET: UserCreditCards/Details/5
        public async Task<IActionResult> CreditCardsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCreditCard = await _context.UserCreditCard
                .Include(u => u.Country)
                .Include(u => u.Province)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCreditCard == null)
            {
                return NotFound();
            }

            return View(userCreditCard);
        }

        // GET: UserCreditCards/Create
        public IActionResult CreditCardsCreate()
        {
            ViewData["CountryId"] = new SelectList(_context.LookupCountry, "Id", "Country");
            ViewData["ProvinceId"] = new SelectList(_context.LookupProvince, "Id", "Province");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: UserCreditCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreditCardsCreate([Bind("Id,UserId,CreditCardType,CreditCardNumber,SecurityCode,ExpirationDate,NameOnCard,Address1,Address2,City,ProvinceId,CountryId,PostalZipCode")] UserCreditCard userCreditCard)
        {
            if (visa.IsMatch(userCreditCard.CreditCardNumber))
            {
                Boolean x = visa.IsMatch(userCreditCard.CreditCardNumber);
                userCreditCard.CreditCardType = "Visa";
            }
            else if (mastercard.IsMatch(userCreditCard.CreditCardNumber))
            {
                Boolean x = mastercard.IsMatch(userCreditCard.CreditCardNumber);
                userCreditCard.CreditCardType = "Mastercard";
            }
            else if (express.IsMatch(userCreditCard.CreditCardNumber))
            {
                Boolean x = express.IsMatch(userCreditCard.CreditCardNumber);
                userCreditCard.CreditCardType = "American Express";
            }
            else
            {
                TempData["message"] = "Invalid Card Type.";
                return Redirect("/Member/CreditCardsCreate");
            }
            if (ModelState.IsValid)
            {
                _context.Add(userCreditCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CreditCardsIndex));
            }
            ViewData["CountryId"] = new SelectList(_context.LookupCountry, "Id", "Country", userCreditCard.CountryId);
            ViewData["ProvinceId"] = new SelectList(_context.LookupProvince, "Id", "Province", userCreditCard.ProvinceId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", userCreditCard.UserId);
            return View(userCreditCard);
        }

        // GET: UserCreditCards/Edit/5
        public async Task<IActionResult> CreditCardsEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCreditCard = await _context.UserCreditCard.FindAsync(id);
            if (userCreditCard == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.LookupCountry, "Id", "Country", userCreditCard.CountryId);
            ViewData["ProvinceId"] = new SelectList(_context.LookupProvince, "Id", "Province", userCreditCard.ProvinceId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", userCreditCard.UserId);
            return View(userCreditCard);
        }

        // POST: UserCreditCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreditCardsEdit(int id, [Bind("Id,UserId,CreditCardType,CreditCardNumber,SecurityCode,ExpirationDate,NameOnCard,Address1,Address2,City,ProvinceId,CountryId,PostalZipCode")] UserCreditCard userCreditCard)
        {
            if (id != userCreditCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCreditCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCreditCardExists(userCreditCard.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CreditCardsIndex));
            }
            ViewData["CountryId"] = new SelectList(_context.LookupCountry, "Id", "Country", userCreditCard.CountryId);
            ViewData["ProvinceId"] = new SelectList(_context.LookupProvince, "Id", "Province", userCreditCard.ProvinceId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", userCreditCard.UserId);
            return View(userCreditCard);
        }

        // GET: UserCreditCards/Delete/5
        public async Task<IActionResult> CreditCardsDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCreditCard = await _context.UserCreditCard
                .Include(u => u.Country)
                .Include(u => u.Province)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCreditCard == null)
            {
                return NotFound();
            }

            return View(userCreditCard);
        }

        // POST: UserCreditCards/Delete/5
        [HttpPost, ActionName("CreditCardsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreditCardsDeleteConfirmed(int id)
        {
            var userCreditCard = await _context.UserCreditCard.FindAsync(id);
            _context.UserCreditCard.Remove(userCreditCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CreditCardsIndex));
        }

        private bool UserCreditCardExists(int id)
        {
            return _context.UserCreditCard.Any(e => e.Id == id);
        }
    }
}
