using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CVGS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace CVGS.Controllers
{
    public class GamesController : Controller
    {
        private readonly CVGSContext _context;

        public GamesController(CVGSContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, int? categoryId, int? platformId)
        {

            var games = from g in _context.Game
                           select g;

            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(g => g.Name.Contains(searchString));
            }
            if (platformId != null)
            {
                games = games.Where(g => g.PlatformId == platformId);
            }
            if (categoryId != null)
            {
                games = games.Where(g => g.CategoryId == categoryId);
            }


            //Set overall ratings
            foreach (Game game in games)
            {
                var reviews = _context.Review
                .Include(a => a.Game)
                .Where(a => a.GameId == game.GameId && a.ApprovedFlag == true)
                .OrderBy(a => a.Date)
                .ToList();

                decimal? overallRating = null;

                if (reviews.Count() > 0)
                {
                    overallRating = (int)reviews.Sum(a => a.Rating) / (decimal)reviews.Count();
                }

                game.Rating = overallRating;
            }


            ViewData["PlatformList"] = new SelectList(_context.LookupPlatform.OrderBy(p => p.Platform), "Id", "Platform");
            ViewData["CategoryList"] = new SelectList(_context.LookupCategory.OrderBy(c => c.Category), "Id", "Category");
            return View(games.ToList());
        }


        public async Task<IActionResult> Details(int? gameId, int? userId)
        {

            if (gameId != null)
            {
                HttpContext.Session.SetString(nameof(gameId), gameId.ToString());
            }
            else if (HttpContext.Session.GetString(nameof(gameId)) != null)
            {
                gameId = Convert.ToInt32(HttpContext.Session.GetString(nameof(gameId)));
            }
            else
            {
                return Redirect("/Games/Index");
            }

            var reviews = _context.Review
                .Include(a => a.Game)
                .Include(a => a.User)
                .Where(a => a.GameId == gameId && a.ApprovedFlag == true)
                .OrderBy(a => a.Date)
                .ToList();

            decimal overallRating = 0;

            if(reviews.Count() > 0)
            {
               overallRating = (int)reviews.Sum(a => a.Rating) / (decimal)reviews.Count();
            }

            var userReview = _context.Review
                .Include(a => a.Game)
                .Include(a => a.User)
                .Where(a => a.GameId == gameId && a.UserId == userId)
                .FirstOrDefault();

            var game = await _context.Game
                .Include(u => u.Platform)
                .Include(u => u.Category)
               .SingleOrDefaultAsync(m => m.GameId == gameId);

            var wishList = _context.UserGameWishList
                    .Include(a => a.Game)
                    .Include(a => a.User)
                    .Where(m => m.GameId == gameId && m.UserId == userId);

            var onWishList = wishList.Count() == 0 ? false: true;

            bool inCart = false;
            List<Game> cartItems = HttpContext.Session.GetObjectFromJson<Game>("cart");

            if(cartItems != null)
            {
                for (int i = 0; i < cartItems.Count; i++)
                {
                    if (cartItems[i].GameId == gameId)
                    {
                        inCart = true;
                        break;
                    }
                }
            }

            bool purchased = false;
            var userOrders = await _context.Order.Where(a => a.UserId == userId).ToListAsync();

            for (int i = 0; i < userOrders.Count; i++)
            {
                if(userOrders[i].GameId == gameId)
                {
                    purchased = true;
                    break;
                }
            }

            GameModel gameModel = new GameModel();
            gameModel.Reviews = reviews;
            gameModel.OverallRating = overallRating;
            gameModel.UserReview = userReview;
            gameModel.Game = game;
            gameModel.OnWishList = onWishList;
            gameModel.InCart = inCart;
            gameModel.Purchased = purchased;

            if (gameModel == null)
            {
                return NotFound();
            }

            return View(gameModel);
        }

        public IActionResult RateGame(int? gameId, int? userId)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateGame([Bind("UserId,GameId,Username,Review1,Rating,Date,ApprovedFlag")]Review review)
        {
            var r = review;

            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
            }

            return View(review);
        }


        public async Task<IActionResult> EditReview(int? userId, int? gameId)
        {
            if (userId == null || gameId == null)
            {
                return NotFound();
            }

            var review = await _context.Review.SingleOrDefaultAsync(m => m.GameId == gameId || m.UserId == userId);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(int reviewId,[Bind("Id,UserId,GameId,Username,Review1,Rating,Date,ApprovedFlag")]Review review)
        {

            if (reviewId != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.GameId, review.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            }
            return View(review);
        }


        public async Task<IActionResult> DeleteReview(int? gameId, int? userId)
        {
            if (gameId == null || userId == null)
            {
                return NotFound();
            }


            var review = await _context.Review
                          .SingleOrDefaultAsync(m => m.GameId == gameId && m.UserId == userId);

            if (review == null)
            {
                return NotFound();
            }

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details));
        }

        public async Task<IActionResult> AddToCart(int userId,int gameId)
        {
            List<Game> games = HttpContext.Session.GetObjectFromJson<Game>("cart");
            var game = await _context.Game.Include(a => a.Platform).Include(a => a.Category).SingleOrDefaultAsync(a => a.GameId == gameId);
            games.Add(game);

            HttpContext.Session.SetObjectAsJson("cart", games);
            HttpContext.Session.SetString("cartCount", games.Count().ToString());


            decimal subTotal = Convert.ToDecimal(HttpContext.Session.GetString("subTotal"));
            decimal tax = Convert.ToDecimal(HttpContext.Session.GetString("tax"));
            decimal total = Convert.ToDecimal(HttpContext.Session.GetString("total"));

            subTotal = subTotal + game.Price;
            tax = subTotal * (decimal)0.15;
            total = subTotal + tax;

            HttpContext.Session.SetString("subTotal", subTotal.ToString());
            HttpContext.Session.SetString("tax", tax.ToString());
            HttpContext.Session.SetString("total", total.ToString());

            return RedirectToAction("Details", new { gameId = gameId, userId = userId });
        }

        public async Task<IActionResult> AddToWishList(int userId, int gameId)
        {

            UserGameWishList record = new UserGameWishList();
            record.GameId = gameId;
            record.UserId = userId;

            if (ModelState.IsValid)
            {
                _context.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { gameId = gameId, userId = userId }  );
            }

            return View();
        }

        private bool ReviewExists(int? gameId, int? userId)
        {
            return _context.Review.Any(e => e.GameId == gameId && e.UserId == userId);
        }

    }
}
