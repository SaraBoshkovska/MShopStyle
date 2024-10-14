using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MShopStyle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MShopStyle.Areas.Identity.Data;

namespace MShopStyle.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<MShopStyleUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            MShopStyleUser user = await UserManager.FindByEmailAsync("admin@mstyle.com");
            if (user == null)
            {
                var User = new MShopStyleUser();
                User.Email = "admin@mstyle.com";
                User.UserName = "admin@mstyle.com";
                string userPWD = "admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
            var roleCheck2 = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck2)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("User"));
            }
            MShopStyleUser user1 = await UserManager.FindByEmailAsync("user1@mstyle.com");
            if (user1 == null)
            {
                var User = new MShopStyleUser();
                User.Email = "user1@mstyle.com";
                User.UserName = "Tony12";
                string userPWD = "toni1234";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                if (chkUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(User, "User");
                }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MShopStyleContext(
               serviceProvider.GetRequiredService<DbContextOptions<MShopStyleContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Artist.Any() || context.Product.Any() || context.Genre.Any() || context.Review.Any())

                {
                    return;
                }


                context.Genre.AddRange(
                    new Genre { GenreName = "Pop" },
                    new Genre { GenreName = "Rock" },
                    new Genre { GenreName = "Hip hop" },
                    new Genre { GenreName = "Metal" }
                   );

                context.SaveChanges();


                context.Artist.AddRange(
                    new Artist
                    {
                        FirstName = "Taylor",
                        LastName = "Swift",
                        ArtistImage = "https://www.rollingstone.com/wp-content/uploads/2023/12/taylor-swift-pennsylvania-era.jpg?w=1581&h=1054&crop=1"

                    },
                    new Artist
                    {
                        FirstName = "Ed",
                        LastName = "Sheeran",
                        ArtistImage = "https://www.nme.com/wp-content/uploads/2023/03/ed-sheeran-2000x1270-2-696x442.jpg"

                    },
                    new Artist
                    {
                        FirstName = "Harry",
                        LastName = "Styles",
                        ArtistImage = "https://static01.nyt.com/images/2022/08/24/arts/23live-styles1/merlin_211759164_aaacb597-755d-4c79-8289-7f6ac43c134f-jumbo.jpg?quality=75&auto=webp"

                    },
                    new Artist
                    {
                        FirstName = "Nirvana",
                        LastName = "",
                        ArtistImage = "https://idsb.tmgrup.com.tr/ly/uploads/images/2021/09/20/thumbs/800x531/146077.jpg?v=1632145483"

                    },

                    new Artist
                    {
                        FirstName = "Coldplay",
                        LastName = "",
                        ArtistImage = "https://images.lifestyleasia.com/wp-content/uploads/sites/5/2023/05/17205156/Coldplay-Hero-2-1600x900.jpg?tr=w-600"

                    },
                    new Artist
                    {
                        FirstName = "Linkin",
                        LastName = "Park",
                        ArtistImage = "https://i.iheart.com/v3/re/new_assets/63e19c7aa5ec4008b89c31bf?ops=contain(1480,0)"

                    },
                    new Artist
                    {
                        FirstName = "Eminem",
                        LastName = "",
                        ArtistImage = "https://concert2025.com/wp-content/uploads/2024/03/Eminem-Concert.jpg.webp"

                    },
                    new Artist
                    {
                        FirstName = "Dr.",
                        LastName = "Dre",
                        ArtistImage = "https://ca-times.brightspotcdn.com/dims4/default/53a63f2/2147483647/strip/true/crop/5329x3553+0+0/resize/1200x800!/format/webp/quality/75/?url=https%3A%2F%2Fcalifornia-times-brightspot.s3.amazonaws.com%2F6b%2F77%2Fd08bcced456d9da49b588f4bbf44%2F914166-sp-0212-super-bowl-13-rcg.jpg"

                    },

                    new Artist
                    {
                        FirstName = "Kendrick",
                        LastName = "Lamar",
                        ArtistImage = "https://www.rollingstone.com/wp-content/uploads/2022/10/kendrick-lamar-live-stream.jpg?w=1581&h=1054&crop=1"

                    },
                     new Artist
                     {
                         FirstName = "Metallica",
                         LastName = "",
                         ArtistImage = "https://media.cnn.com/api/v1/images/stellar/prod/230902114235-metallica-los-angeles-0827-restricted.jpg?c=16x9&q=h_653,w_1160,c_fill/f_webp"

                     },
                    new Artist
                    {
                        FirstName = "Led",
                        LastName = "Zeppelin",
                        ArtistImage = "https://upload.wikimedia.org/wikipedia/commons/d/d3/Jimmy_Page_with_Robert_Plant_2_-_Led_Zeppelin_-_1977.jpg"

                    },
                    new Artist
                    {
                        FirstName = "Iron",
                        LastName = "Maiden",
                        ArtistImage = "https://www.cincinnati.com/gcdn/presto/2019/08/08/PCIN/5c37ebbf-c953-40c8-9666-7a3cb77a7a04-HELSINKI_SHOW_1_2018_IM_-58061.jpg?width=660&height=441&fit=crop&format=pjpg&auto=webp"

                    }
                    );
                context.SaveChanges();


                context.ArtistGenre.AddRange
                (
                    new ArtistGenre { ArtistId = 1, GenreId = 1 },
                     new ArtistGenre { ArtistId = 2, GenreId = 1 },
                      new ArtistGenre { ArtistId = 3, GenreId = 1 },
                       new ArtistGenre { ArtistId = 4, GenreId = 2 },
                        new ArtistGenre { ArtistId = 5, GenreId = 2 },
                         new ArtistGenre { ArtistId = 6, GenreId = 2 },
                          new ArtistGenre { ArtistId = 7, GenreId = 3 },
                           new ArtistGenre { ArtistId = 8, GenreId = 3 },
                            new ArtistGenre { ArtistId = 9, GenreId = 3 },
                             new ArtistGenre { ArtistId = 10, GenreId = 4 },
                              new ArtistGenre { ArtistId = 11, GenreId = 4 },
                               new ArtistGenre { ArtistId = 12, GenreId = 4 }
                );
                context.SaveChanges();

                context.Product.AddRange
                (
                    new Product
                    {
                        Name = "Poster",
                        Price = "$10",
                        Description = "Taylor Swift, The Eras Tour, Poster, Canvas Art Poster Bedroom Decor Posters 12x18inch(30x45cm)",
                        ImageProduct = "https://i.ebayimg.com/images/g/WYEAAOSwXUplFuIg/s-l1600.webp",
                        ArtistId = context.Artist.Single(a => a.FirstName == "Taylor" && a.LastName == "Swift").Id,
                        Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }
                    },
                    new Product
                    {
                        Name = "CD",
                        Price = "$13",
                        Description = "The Tortured Poets Department CD + Bonus Track “The Manuscript",
                        ImageProduct = "https://store.taylorswift.com/cdn/shop/files/4iOaQsRrtFFgsmmdZf6njBG7PjLl8SON.png?v=1707095354&width=2048",
                        ArtistId = context.Artist.Single(a => a.FirstName == "Taylor" && a.LastName == "Swift").Id,
                        Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }

                    },
                    new Product
                    {
                        Name = "CD",
                        Price = "$13",
                        Description = "Limited Edition CD including the original 12-track album plus 9 bonus tracks. ",
                        ImageProduct = "https://store.edsheeran.com/dw/image/v2/BHCC_PRD/on/demandware.static/-/Sites-warner-master/default/dw9790633b/ed%20sheeran/UK/X%2010th%20Anniversary/Music/EdSheeran_X10_CD_Shot.png?sw=550&sh=550&sm=fit",
                        ArtistId = context.Artist.Single(a => a.FirstName == "Ed" && a.LastName == "Sheeran").Id,
                        Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }


                    },
                    new Product
                    {
                        Name = "CD",
                        Price = "$20",
                        Description = "Commemorating the 30th anniversary of Nirvana’s seminal 1991 release, " +
                        "the Nevermind 2CD Deluxe Edition contains the newly remastered album" +
                        " from the original analog tapes and selections from 4 concerts ",
                        ImageProduct = "https://m.media-amazon.com/images/I/81qhnvqrM2L._SL1500_.jpg",
                        ArtistId = context.Artist.Single(a => a.FirstName == "Nirvana" && a.LastName == "").Id,
                        Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }

                    },
                    new Product
                    {
                        Name = "CD",
                        Price = "$20",
                        Description = "LIMITED EDITION, ONE-TIME NOTEBOOK EDITION ECOCD PRESSING OF MOON MUSiC.",
                        ImageProduct = "https://powermaxx2-i04.mycdn.no/mysimgprod/powermaxx2_mystore_no/images/jbRpu_Warner_Music_Coldplay_-_Moon_Music__Softpak__1.jpg/w1200h1200.jpg",
                        ArtistId = context.Artist.Single(a => a.FirstName == "Coldplay" && a.LastName == "").Id,
                        Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }

                    },
                    new Product
                    {
                        Name = "CD",
                        Price = "$13",
                        Description = "Papercuts standard softpack compact disc.",
                        ImageProduct = "https://linkinpark.warnerrecords.com/dw/image/v2/BHCC_PRD/on/demandware.static/-/Sites-warner-master/default/dwd88a732e/Linkin%20Park/LP%20PC%20Standard%20CD.jpg?sw=550&sh=550&sm=fit",
                        ArtistId = context.Artist.Single(a => a.FirstName == "Linkin" && a.LastName == "Park").Id,

                    },
                    new Product
                    {
                        Name = "CD",
                        Price = "$14",
                        Description = "THE EMINEM SHOW CD. EXPLICIT.",
                        ImageProduct = "https://upload.wikimedia.org/wikipedia/en/3/35/The_Eminem_Show.jpg",
                        ArtistId = context.Artist.Single(a => a.FirstName == "Eminem" && a.LastName == "").Id,
                        Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }

                    },
                    new Product
                    {
                        Name = "CD",
                        Price = "$15",
                        Description = "THE CHRONIC CD - 1992",
                        ImageProduct = "https://upload.wikimedia.org/wikipedia/en/1/19/Dr.DreTheChronic.jpg",
                        ArtistId = context.Artist.Single(a => a.FirstName == "Dr." && a.LastName == "Dre").Id,

                    },
                     new Product
                     {
                         Name = "CD",
                         Price = "$14",
                         Description = "'MR. MORALE & THE BIG STEPPERS' CD",
                         ImageProduct = "https://interscope.com/cdn/shop/products/KLCDpng_1024x1024.png?v=1652741303",
                         ArtistId = context.Artist.Single(a => a.FirstName == "Kendrick" && a.LastName == "Lamar").Id,
                         Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }

                     },
                      new Product
                      {
                          Name = "CD",
                          Price = "$15",
                          Description = "72 SEASONS CD",
                          ImageProduct = "https://www.metallica.com/dw/image/v2/BCPJ_PRD/on/demandware.static/-/Sites-met-master/default/dw4b8dcdc7/images/hi-res/72CD.jpg?sw=650",
                          ArtistId = context.Artist.Single(a => a.FirstName == "Metallica" && a.LastName == "").Id,
                          Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }

                      },
                       new Product
                       {
                           Name = "CD",
                           Price = "$12",
                           Description = "Led Zeppelin III Original",
                           ImageProduct = "https://m.media-amazon.com/images/I/81lVMzMGeYL._AC_SL1425_.jpg",
                           ArtistId = context.Artist.Single(a => a.FirstName == "Led" && a.LastName == "Zeppelin").Id,
                           Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }

                       },
                        new Product
                        {
                            Name = "CD",
                            Price = "$7",
                            Description = "The Number of the Beast",
                            ImageProduct = "https://m.media-amazon.com/images/I/81wajmBj+6L._SL1500_.jpg",
                            ArtistId = context.Artist.Single(a => a.FirstName == "Iron" && a.LastName == "Maiden").Id,
                            Reviews = new List<Review>
                        {
                            new Review{ProductId=4,AppUser="Roxy23", Comment="Best Album Ever", Rating=5}
                        }

                        }
                );
                context.SaveChanges();
            }
        }
    }
}
