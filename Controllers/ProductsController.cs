using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dotnet_app.Data;
using dotnet_app.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;

namespace dotnet_app.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider fileProvider;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ProductsController(ApplicationDbContext context, IFileProvider fileprovider, IWebHostEnvironment env)
        {
            _context = context;
            fileProvider = fileprovider;
            hostingEnvironment = env;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProduct,ProductCode,ProductType,Description,UnitPrice,QtyInStock,YoutubeVideo")] Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var beginVideoId = product.YoutubeVideo.IndexOf("=");
                product.YoutubeVideo = product.YoutubeVideo.Substring(beginVideoId + 1, 11);
                _context.Add(product);
                await _context.SaveChangesAsync();

                // code for upload image
                if (file != null || file.Length != 0)
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    var newFilename = product.IdProduct + " " + String.Format("{0:d}", (DateTime.Now.Ticks / 10) % 100000000) +fi.Extension;
                    var webPath = hostingEnvironment.WebRootPath;
                    var path = Path.Combine("", webPath + @"/imageFiles/" + newFilename);
                    var pathToSave = @"/imageFiles/" + newFilename;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    product.ImagePath = pathToSave;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProduct,ProductCode,ProductType,Description,UnitPrice,QtyInStock")] Product product)
        {
            if (id != product.IdProduct)
            {
                return NotFound();
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
                    if (!ProductExists(product.IdProduct))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.IdProduct == id);
        }
    }
}
