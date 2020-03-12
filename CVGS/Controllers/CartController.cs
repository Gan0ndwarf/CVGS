using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;

namespace CVGS.Controllers
{
    public class CartController : Controller
    {
        private List<Game> orderedGames;
        private readonly CVGSContext _context;

        public CartController(CVGSContext context)
        {
            _context = context;
            orderedGames = new List<Game>();
        }

        public ActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<Game>("cart");
            return View(cart);
        }


        public ActionResult Remove(int gameId)
        {
            int index = 0;
            decimal subTotal = (decimal)0.00;
            decimal tax = (decimal)0.00;
            decimal total = (decimal)0.00;

            List<Game> cart = HttpContext.Session.GetObjectFromJson<Game>("cart");

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].GameId == gameId)
                {
                    index = i;
                }
            }

            cart.RemoveAt(index);
            HttpContext.Session.SetObjectAsJson("cart", cart);
            HttpContext.Session.SetString("cartCount", cart.Count.ToString());

            if (cart.Count > 0)
            {
                for (int i = 0; i < cart.Count; i++)
                {
                    subTotal += cart[i].Price;
                }
                tax = subTotal * (decimal)0.15;
                total = subTotal + tax;
            }

            HttpContext.Session.SetString("subTotal", subTotal.ToString());
            HttpContext.Session.SetString("tax", tax.ToString());
            HttpContext.Session.SetString("total", total.ToString());

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ProceedToBilling(int? userId)
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

            var userCreditCards = await _context.UserCreditCard.Where(a => a.UserId == userId).ToListAsync();

            List<CreditCardModel> userCards = new List<CreditCardModel>();
            for (int i = 0; i < userCreditCards.Count; i++)
            {
                CreditCardModel cardModel = new CreditCardModel();
                cardModel.Id = userCreditCards[i].Id;
                cardModel.UserId = userCreditCards[i].UserId;
                cardModel.CreditCardType = userCreditCards[i].CreditCardType;
                cardModel.CreditCardNumber = userCreditCards[i].CreditCardNumber;
                cardModel.SecurityCode = userCreditCards[i].SecurityCode;
                cardModel.Month = userCreditCards[i].ExpirationDate.Month;
                cardModel.Year = userCreditCards[i].ExpirationDate.Year;
                cardModel.NameOnCard = userCreditCards[i].NameOnCard;
                cardModel.Address1 = userCreditCards[i].Address1;
                cardModel.Address2 = userCreditCards[i].Address2;
                cardModel.City = userCreditCards[i].City;
                cardModel.ProvinceId = userCreditCards[i].ProvinceId;
                cardModel.CountryId = userCreditCards[i].CountryId;
                cardModel.PostalZipCode = userCreditCards[i].PostalZipCode;

                userCards.Add(cardModel);
            }

            CreditCardModel blankModel = new CreditCardModel();
            blankModel.Id = null;
            blankModel.UserId = userId;
            blankModel.CreditCardType = null;
            blankModel.CreditCardNumber = null;
            blankModel.SecurityCode = null;
            blankModel.Month = DateTime.Now.Month;
            blankModel.Year = DateTime.Now.Year;
            blankModel.NameOnCard = null;
            blankModel.Address1 = null;
            blankModel.Address2 = null;
            blankModel.City = null;
            blankModel.ProvinceId = null;
            blankModel.CountryId = null;
            blankModel.PostalZipCode = null;


            PayBillModel model = new PayBillModel();
            model.UserCards = userCards;
            model.AddCard = blankModel;

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "VISA", Value = "VISA" });
            items.Add(new SelectListItem { Text = "Mastercard", Value = "Mastercard" });
            items.Add(new SelectListItem { Text = "American Express", Value = "American Express" });
            ViewData["CardTypeList"] = new SelectList(items);
            ViewData["ProvinceList"] = new SelectList(_context.LookupProvince.OrderBy(p => p.Id), "Id", "Province");
            ViewData["CountryList"] = new SelectList(_context.LookupCountry.OrderBy(p => p.Id), "Id", "Country");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProceedToBilling([Bind("UserCards,AddCard")]PayBillModel model)
        {

            if (ModelState.IsValid)
            {
                var cart = HttpContext.Session.GetObjectFromJson<Game>("cart");

                for (int i = 0; i < cart.Count; i++)
                {

                    Order order = new Order();
                    order.UserId = model.AddCard.UserId;
                    order.GameId = cart[i].GameId;
                    order.Date = DateTime.Now;

                    try
                    {
                        _context.Update(order);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        Console.WriteLine(e.Message);
                    }                 
                }

                return RedirectToAction("OrderConfirmation");
            }

            ViewData["ProvinceList"] = new SelectList(_context.LookupProvince.OrderBy(p => p.Id), "Id", "Province");
            ViewData["CountryList"] = new SelectList(_context.LookupCountry.OrderBy(p => p.Id), "Id", "Country");

            return View(model);
        }

        public ActionResult OrderConfirmation()
        {
            orderedGames = HttpContext.Session.GetObjectFromJson<Game>("cart");
            List<Game> games = new List<Game>();
            HttpContext.Session.SetObjectAsJson("cart", games);
            HttpContext.Session.SetString("cartCount", 0.ToString());

            HttpContext.Session.SetString("subTotal", "0.00");
            HttpContext.Session.SetString("tax", "0.00");
            HttpContext.Session.SetString("total", "0.00");

            return View(orderedGames);
        }

        public void DownloadGame()
        {
            StringBuilder sb = new StringBuilder();
            string output = "Output";
            sb.Append(output);
            sb.Append("\r\n");

            string text = sb.ToString();

            Response.Clear();
            Response.Headers.Clear();

            Response.Headers.Append("Content-Length", text.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Headers.Append("Content-Disposition", "attachment;filename=\"game.txt\"");

            Response.WriteAsync(text);
        }
    }
}