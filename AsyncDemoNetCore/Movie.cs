using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncDemoNetCore
{
    public sealed class Movie
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
    }
}
