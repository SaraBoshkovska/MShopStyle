using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using MShopStyle.Models;


namespace MShopStyle.ViewModels
{
    public class CreateArtistViewModel
    {
        public Artist artist { get; set; }


        public IFormFile? ArtistImage { get; set; }
        public IEnumerable<int>? SelectedGenres { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
    }
}
