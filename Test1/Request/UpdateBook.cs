using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test1.Models;

namespace Test1.Request
{
    public class UpdateBook
    {
        public Book Book { get; set; }

        public List<string> Categories { get; set; }
    }
}
