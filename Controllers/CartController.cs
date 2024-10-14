
using Microsoft.AspNetCore.Mvc;
using MShopStyle.Models;
using System.Collections.Generic;
using System.Linq;

namespace MShopStyle.Controllers
{
    public class CartController : Controller
    {
        private static List<CheckoutItemViewModel> cartItems = new List<CheckoutItemViewModel>();

        [HttpPost]
        public IActionResult AddToCart(int productId, string productName, string productPrice, string productImage)
        {
            var existingItem = cartItems.FirstOrDefault(i => i.Product.Id == productId);
            if (existingItem != null)
            {
                if (existingItem.Quantity < 4) 
                {
                    existingItem.Quantity++;
                }
            }
            else
            {
                if (cartItems.Count < 4) 
                {
                    var product = new Product
                    {
                        Id = productId,
                        Name = productName,
                        Price = productPrice,
                        ImageProduct = productImage
                    };
                    cartItems.Add(new CheckoutItemViewModel { Product = product, Quantity = 1 });
                }
            }
            return Json(new { success = true });
        }

        public IActionResult CheckoutView()
        {
            var model = new CheckoutViewModel
            {
                Items = cartItems,
                CheckoutInfo = new CheckoutModel()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitCheckout(CheckoutModel checkoutInfo)
        {
            // Clear the cart
            cartItems.Clear();

            // Set a success message in TempData to show on the next page
            TempData["SuccessMessage"] = "Your order is completed and will arrive in 7 business days.";

            // Redirect to the CheckoutComplete view
            return RedirectToAction("CheckoutComplete");
        }
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var itemToRemove = cartItems.FirstOrDefault(i => i.Product.Id == productId);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
            }
            return Json(new { success = true });
        }

        public IActionResult CheckoutComplete()
        {
            return View();
        }
    }
}
