using BlogBusinessLogic.BindingModels;
using BlogBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogBusinessLogic.Interfaces
{
    public interface IBlogLogic
    {
        void CreateOrUpdate(BlogBindingModel model);
        List<BlogViewModel> Read(BlogBindingModel model);
        void Delete(BlogBindingModel model);
    }
}
