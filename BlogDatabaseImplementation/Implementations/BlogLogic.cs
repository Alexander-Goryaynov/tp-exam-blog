using BlogBusinessLogic.BindingModels;
using BlogBusinessLogic.Interfaces;
using BlogBusinessLogic.ViewModels;
using BlogDatabaseImplementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogDatabaseImplementation.Implementations
{
    public class BlogLogic : IBlogLogic
    {
        public void CreateOrUpdate(BlogBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                Blog element;
                element = context.Blogs.FirstOrDefault(rec => rec.Name == model.Name &&
                    rec.Id != model.Id);
                if (element != null)
                {
                    throw new Exception("Блог с таким названием уже существует");
                }
                if (model.Id.HasValue)
                {
                    element = context.Blogs.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Blog();
                    context.Blogs.Add(element);
                }
                element.Author = model.Author;
                element.CreationDate = model.CreationDate;
                element.Name = model.Name;
                context.SaveChanges();
            }
        }

        public void Delete(BlogBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                var element = context.Blogs.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Blogs.Remove(element);
                    context.SaveChanges();
                } 
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public List<BlogViewModel> Read(BlogBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                return context.Blogs
                    .Where(rec => model == null || rec.Id == model.Id)
                    .Select(rec => new BlogViewModel
                    {
                        Id = rec.Id,
                        Name = rec.Name,
                        Author = rec.Author,
                        CreationDate = rec.CreationDate
                    })
                    .ToList();
            }
        }
    }
}
