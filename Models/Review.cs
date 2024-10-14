using System.ComponentModel.DataAnnotations;

namespace MShopStyle.Models

{
    public class Review
    {
        public int? Id { get; set; }


        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        [StringLength(450)]
        public string? AppUser { get; set; }

        [StringLength(500)]
        [Display(Name = "Comment")]
        public string? Comment { get; set; }


        [Range(0, 5)]
        public int? Rating { get; set; }
    }
}
