using Microsoft.AspNetCore.Mvc.Rendering;
using MShopStyle.Models;
namespace MShopStyle.ViewModels
{
    public class ArtistFilterViewModel
    {
        public IList<Artist> Artists { get; set; }

        public SelectList Genres { get; set; }

        public string NameFilter { get; set; }

        public int? SelectedGenreId { get; set; }
    }
}
