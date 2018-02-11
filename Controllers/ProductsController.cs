using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TuyetWebshop.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TuyetWebshop.Controllers
{
    public class ProductsController : Controller
    {
        // GET: /<controller>/
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GenreSortParm"] = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Genre";

            var search = from s in _context.Product.Include(p => p.Category)
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                search = search.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    search = search.OrderByDescending(s => s.Name);
                    break;
                case "Genre":
                    search = search.OrderBy(s => s.Category);
                    break;
                case "genre_desc":
                    search = search.OrderByDescending(s => s.Category);
                    break;
                case "Price":
                    search = search.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    search = search.OrderByDescending(s => s.Price);
                    break;
                default:
                    search = search.OrderBy(s => s.Name);
                    break;
            }
            return View(await search.ToListAsync());
        }
    }
}
