using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TuyetWebshop.Models;
using TuyetWebshop.Data;

namespace TuyetWebshop.Controllers
{
    public class CategorieAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategorieAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategorieAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Catgory.ToListAsync());
        }

        // GET: CategorieAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catgory = await _context.Catgory
                .SingleOrDefaultAsync(m => m.Id == id);
            if (catgory == null)
            {
                return NotFound();
            }

            return View(catgory);
        }

        // GET: CategorieAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategorieAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Catgory catgory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catgory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catgory);
        }

        // GET: CategorieAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catgory = await _context.Catgory.SingleOrDefaultAsync(m => m.Id == id);
            if (catgory == null)
            {
                return NotFound();
            }
            return View(catgory);
        }

        // POST: CategorieAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Catgory catgory)
        {
            if (id != catgory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catgory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatgoryExists(catgory.Id))
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
            return View(catgory);
        }

        // GET: CategorieAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catgory = await _context.Catgory
                .SingleOrDefaultAsync(m => m.Id == id);
            if (catgory == null)
            {
                return NotFound();
            }

            return View(catgory);
        }

        // POST: CategorieAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catgory = await _context.Catgory.SingleOrDefaultAsync(m => m.Id == id);
            _context.Catgory.Remove(catgory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatgoryExists(int id)
        {
            return _context.Catgory.Any(e => e.Id == id);
        }
    }
}
