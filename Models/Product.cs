using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuyetWebshop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public Catgory Category { get; set; }
        public List<Rating> Ratings { get; set; }
        
        public List<string> GetImages()
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(Image))
            {
                result.AddRange(Image.Split(","));
            }

            return result;
        }
    }
}
