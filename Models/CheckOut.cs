
using System.ComponentModel.DataAnnotations;

namespace MShopStyle.Models
{
    public class CheckoutModel
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}