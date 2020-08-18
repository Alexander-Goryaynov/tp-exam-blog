using System;
using System.Collections.Generic;
using System.Text;

namespace BlogBusinessLogic.ViewModels
{
    public class ReportBlogCommentsViewModel
    {
        public string BlogName { get; set; }
        public string BlogCreationDate { get; set; }
        public string CommentHeader { get; set; }
        public string CommentAuthor { get; set; }
        public string CommentCreationDate { get; set; }
    }
}
