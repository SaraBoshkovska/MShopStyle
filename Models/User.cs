namespace MShopStyle.Models
{
    public class User
    {
        public int? Id { get; set; }

        public string? AppUser { get; set; }

        public int? ProductId { get; set; }

        public Product? Product { get; set; }

    }
}
