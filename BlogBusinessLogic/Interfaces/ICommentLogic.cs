using BlogBusinessLogic.BindingModels;
using BlogBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogBusinessLogic.Interfaces
{
    public interface ICommentLogic
    {
        void CreateOrUpdate(CommentBindingModel model);
        List<CommentViewModel> Read(CommentBindingModel model);
        void Delete(CommentBindingModel model);
    }
}
