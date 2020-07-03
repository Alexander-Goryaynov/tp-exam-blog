using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BlogBusinessLogic.ViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Автор")]
        public string Author { get; set; }
        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }
    }
}
