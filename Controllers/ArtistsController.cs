using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MShopStyle.Interfaces;
using MShopStyle.Models;
using MShopStyle.ViewModels;

namespace MShopStyle.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly MShopStyleContext _context;
        private readonly IBufferedFileUploadService _bufferedFileUploadService;

        public ArtistsController(MShopStyleContext context, IBufferedFileUploadService bufferedFileUploadService)
        {
            _context = context;
            _bufferedFileUploadService = bufferedFileUploadService;
        }

        // GET: Artists
        public async Task<IActionResult> Index(string NameFilter, int? SelectedGenreId)
        {
            IQueryable<Artist> artist = _context.Artist
                 .Include(a => a.ArtistGenres)
                 .ThenInclude(ag => ag.Genre)
                 .AsQueryable();

            if (SelectedGenreId.HasValue)
            {
                artist = artist.Where(a => a.ArtistGenres.Any(ag => ag.GenreId == SelectedGenreId));
            }

            var artists = await artist.ToListAsync();
            if (!string.IsNullOrEmpty(NameFilter))
            {
                artists = artists.Where(a => a.FullName.Contains(NameFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var viewModel = new ArtistFilterViewModel
            {
                Artists = artists,
                Genres = new SelectList(_context.Genre, "Id", "GenreName"),
                NameFilter = NameFilter,
                SelectedGenreId = SelectedGenreId
            };
            return View(viewModel);
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .Include(a => a.ArtistGenres)
                .ThenInclude(ag => ag.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // GET: Artists/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {


            var genres = await _context.Genre.OrderBy(s => s.GenreName).ToListAsync();

            CreateArtistViewModel viewmodel = new CreateArtistViewModel
            {
                artist = new Artist(),
                GenreList = new MultiSelectList(genres, "Id", "GenreName"),
                SelectedGenres = new List<int>()
            };

            return View(viewmodel);

        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateArtistViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Artist artist = new Artist
                {
                    FirstName = viewModel.artist.FirstName,
                    LastName = viewModel.artist.LastName,

                };
                if (viewModel.ArtistImage != null && viewModel.ArtistImage.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images", viewModel.ArtistImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ArtistImage.CopyToAsync(stream);
                    }

                    artist.ArtistImage = "/images/" + viewModel.ArtistImage.FileName;
                }

                if (viewModel.SelectedGenres != null)
                {
                    foreach (int genreId in viewModel.SelectedGenres)
                    {
                        artist.ArtistGenres.Add(new ArtistGenre { GenreId = genreId, Artist = artist });
                    }
                }
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var genres = await _context.Genre.OrderBy(s => s.GenreName).ToListAsync();
            viewModel.GenreList = new MultiSelectList(genres, "Id", "GenreName");
            return View(viewModel);

        }

        // GET: Artists/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = _context.Artist.Where(a => a.Id == id).Include(a => a.ArtistGenres).First();
            if (artist == null)
            {
                return NotFound();
            }

            var genres = _context.Genre.AsEnumerable();
            genres = genres.OrderBy(s => s.GenreName);
            CreateArtistViewModel viewmodel = new CreateArtistViewModel
            {
                artist = artist,
                GenreList = new MultiSelectList(genres, "Id", "GenreName"),
                SelectedGenres = artist.ArtistGenres.Select(sg => sg.GenreId)
            };
            return View(viewmodel);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateArtistViewModel viewmodel)
        {
            if (id != viewmodel.artist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(viewmodel.artist);
                await _context.SaveChangesAsync();

                IEnumerable<int> newGenreList = viewmodel.SelectedGenres;
                IEnumerable<int> prevGenreList = _context.ArtistGenre.Where(s => s.ArtistId == id).Select(s => s.GenreId);
                IQueryable<ArtistGenre> toBeRemoved = _context.ArtistGenre.Where(s => s.ArtistId == id);
                if (newGenreList != null)
                {
                    toBeRemoved = toBeRemoved.Where(s => !newGenreList.Contains(s.GenreId));
                    foreach (int genreId in newGenreList)
                    {
                        if (!prevGenreList.Any(s => s == genreId))
                        {
                            _context.ArtistGenre.Add(new ArtistGenre { GenreId = genreId, ArtistId = id });
                        }
                    }
                }
                if (viewmodel.ArtistImage != null)
                {
                    await _bufferedFileUploadService.UploadFile(viewmodel.ArtistImage);
                    viewmodel.artist.ArtistImage = "/images/" + viewmodel.ArtistImage.FileName;
                }
                _context.ArtistGenre.RemoveRange(toBeRemoved);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewmodel);
        }

        // GET: Artists/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _context.Artist.FindAsync(id);
            if (artist != null)
            {
                _context.Artist.Remove(artist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
            return _context.Artist.Any(e => e.Id == id);
        }
    }
}
