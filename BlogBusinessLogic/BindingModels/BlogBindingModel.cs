using System;
using System.Collections.Generic;
using System.Text;

namespace BlogBusinessLogic.BindingModels
{
    public class BlogBindingModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
