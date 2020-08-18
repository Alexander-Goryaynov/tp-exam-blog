using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlogDatabaseImplementation.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public int BlogId { get; set; }
        [ForeignKey("BlogId")]
        public Blog Blog { get; set; }
    }
}
