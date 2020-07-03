using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDatabaseImplementation.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
