using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveWall
{
    public class Movie
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public Movie(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
