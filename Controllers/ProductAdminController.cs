﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TuyetWebshop.Models;
using TuyetWebshop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace TuyetWebshop.Controllers
{
    [Authorize]
    public class ProductAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public ProductAdminController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: ProductAdmin
        public async Task<IActionResult> Index()
        {
            var TuyetWebshopContext = _context.Product.Include(p => p.Category);
            return View(await TuyetWebshopContext.ToListAsync());
        }

        // GET: ProductAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: ProductAdmin/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Catgory>(), "Id", "Name");
            return View();
        }

        // POST: ProductAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Image,Name,Price,CategoryId")] Product product, List<IFormFile> imageUploads)
        {
            if (ModelState.IsValid)
            {
                if (imageUploads != null && imageUploads.Count > 0)
                {
                    var files = new List<string>();
                    foreach (var imageUpload in imageUploads)
                    {
                        var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
                        string newFilename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(imageUpload.FileName)}";
                        using (var fileStream = new FileStream(Path.Combine(uploadPath, newFilename), FileMode.Create))
                        {
                            await imageUpload.CopyToAsync(fileStream);
                        }

                        files.Add($"/uploads/{newFilename}");
                    }

                    product.Image = string.Join(",", files);
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Set<Catgory>(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: ProductAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Catgory>(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: ProductAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,Name,Price,CategoryId")] Product product, List<IFormFile> imageUploads)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageUploads != null && imageUploads.Count > 0)
                    {
                        var files = new List<string>();
                        foreach (var imageUpload in imageUploads)
                        {
                            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
                            string newFilename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(imageUpload.FileName)}";
                            using (var fileStream = new FileStream(Path.Combine(uploadPath, newFilename), FileMode.Create))
                            {
                                await imageUpload.CopyToAsync(fileStream);
                            }

                            files.Add($"/uploads/{newFilename}");
                        }

                        product.Image = string.Join(",", files);
                    }

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
            ViewData["CategoryId"] = new SelectList(_context.Set<Catgory>(), "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: ProductAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: ProductAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
