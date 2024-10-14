namespace MShopStyle.Models

{
    public class Artist
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ArtistImage { get; set; }

        public ICollection<ArtistGenre> ArtistGenres { get; set; }

        public Artist()
        {
            ArtistGenres = new List<ArtistGenre>();
        }

        public ICollection<Product>? Products { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }

        }
        public string? Bio { get; set; }

        public string? Discography { get; set; }

        public string? Awards { get; set; }

    }
}
