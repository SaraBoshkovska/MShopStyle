
using System.Collections.Generic;

namespace MShopStyle.Models
{
    public class CheckoutItemViewModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice // Add this property for total price calculation
        {
            get
            {
                // Convert price from string to decimal and multiply by quantity
                return decimal.TryParse(Product.Price, out decimal price) ? price * Quantity : 0;
            }
        }
    }

    public class CheckoutViewModel
    {
        public List<CheckoutItemViewModel> Items { get; set; } = new List<CheckoutItemViewModel>();
        public CheckoutModel CheckoutInfo { get; set; } = new CheckoutModel();
    }
}
