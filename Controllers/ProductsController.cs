using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MShopStyle.Interfaces;
using MShopStyle.Services;
using MShopStyle.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MShopStyle.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MShopStyleContext _context;
        private readonly IBufferedFileUploadService _bufferedFileUploadService;
        private readonly UserManager<IdentityUser> _userManager;
        public ProductsController(MShopStyleContext context, IBufferedFileUploadService bufferedFileUploadService)
        {
            _context = context;
            _bufferedFileUploadService = bufferedFileUploadService;
        }

        // GET: Products
        public async Task<IActionResult> Index(string category, int? artistId)
        {
            var products = from p in _context.Product.Include(p => p.Artist)
                           select p;

            if (artistId.HasValue)
            {
                products = products.Where(p => p.ArtistId == artistId.Value);
                ViewData["ArtistId"] = artistId.Value;
            }

            if (!string.IsNullOrEmpty(category) && category != "all")
            {

                string lowerCaseCategory = category.ToLower();
                products = products.Where(p => p.Name.ToLower() == lowerCaseCategory);
            }

            ViewData["SelectedCategory"] = category ?? "all";

            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Set<Artist>(), "Id", "FullName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,ArtistId")] Product product, IFormFile ImageProduct)
        {
            if (ImageProduct != null && ImageProduct.Length > 0)
            {
                var filePath = await _bufferedFileUploadService.UploadFile(ImageProduct);
                product.ImageProduct = "/images/" + ImageProduct.FileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ArtistId"] = new SelectList(_context.Set<Artist>(), "Id", "FullName", product.ArtistId);
            return View(product);
        }



        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "FullName", product.ArtistId);
            return View(product);
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,ArtistId,ImageProduct")] Product product, IFormFile ImageProduct)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ImageProduct != null && ImageProduct.Length > 0)
            {
                var filePath = await _bufferedFileUploadService.UploadFile(ImageProduct);
                product.ImageProduct = "/images/" + ImageProduct.FileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "FullName", product.ArtistId);
            return View(product);
        }

        public IActionResult BuyingPage(int id)
        {
            var product = _context.Product
       .Include(p => p.Artist)
        .Include(p => p.Reviews)
       .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(); 
            }

           

            return View(product); 
        }
        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            _context.Review.RemoveRange(product.Reviews);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]

        public IActionResult SubmitReview(Review review)
        {
      
            if (ModelState.IsValid)
            {
                _context.Review.Add(review);
                _context.SaveChanges();

            
                return RedirectToAction("BuyingPage", new { id = review.ProductId });
            }

       
            var product = _context.Product.Include(p => p.Reviews).FirstOrDefault(p => p.Id == review.ProductId);
            return View("BuyingPage", product);
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
       
        public IActionResult RemoveReview(int ReviewId)
        {
            var review = _context.Review.FirstOrDefault(r => r.Id == ReviewId);
            if (review != null)
            {
                _context.Review.Remove(review);
                _context.SaveChanges();
            }

            
            return RedirectToAction("BuyingPage", new { id = review.ProductId });
        }
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
