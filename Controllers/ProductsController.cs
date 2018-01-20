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

        public async Task<IActionResult> Index()
        {
            var TuyetWebshopContext = _context.Product.Include(p => p.Category);
            return View(await TuyetWebshopContext.ToListAsync());
        }
    }
}
