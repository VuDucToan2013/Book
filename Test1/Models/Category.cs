using System;
using System.Collections.Generic;

#nullable disable

namespace Test1.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int? BookId { get; set; }

        public virtual Book Book { get; set; }
    }
}
