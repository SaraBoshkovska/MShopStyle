using Microsoft.AspNetCore.Mvc.ViewEngines;
namespace MShopStyle.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public string Price { get; set; }

        public string? Description { get; set; }


        public string? ImageProduct { get; set; }

        public int ArtistId { get; set; }
        public Artist? Artist { get; set; }

        public ICollection<Review>? Reviews { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
