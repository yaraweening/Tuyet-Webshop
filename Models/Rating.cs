using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuyetWebshop.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public Product Products { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
