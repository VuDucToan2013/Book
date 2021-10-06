using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test1.Request
{
    public class BookRequest
    {
        public string BookName { get; set; }

        public List<string> categoryNames { get; set; }
    }
}
