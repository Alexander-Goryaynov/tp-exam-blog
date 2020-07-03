using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BlogBusinessLogic.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        [DisplayName("Заголовок")]
        public string Header { get; set; }
        [DisplayName("Текст")]
        public string Text { get; set; }
        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }
        [DisplayName("Автор")]
        public string Author { get; set; }
        public int BlogId { get; set; }
        [DisplayName("Блог")]
        public string BlogName { get; set; }
    }
}
