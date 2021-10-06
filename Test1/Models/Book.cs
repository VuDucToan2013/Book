using System;
using System.Collections.Generic;

#nullable disable

namespace Test1.Models
{
    public partial class Book
    {
        public Book()
        {
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }
        public string BookName { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
