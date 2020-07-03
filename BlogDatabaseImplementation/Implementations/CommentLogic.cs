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
    public class CommentLogic : ICommentLogic
    {
        public void CreateOrUpdate(CommentBindingModel model)
        {
            using(var context = new DatabaseContext())
            {
                Comment element;
                if (model.Id.HasValue)
                {
                    element = context.Comments.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Comment();
                    context.Comments.Add(element);
                }
                element.Author = model.Author;
                element.BlogId = model.BlogId;
                element.CreationDate = model.CreationDate;
                element.Header = model.Header;
                element.Text = model.Text;
                context.SaveChanges();
            }
        }

        public void Delete(CommentBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                var element = context.Comments.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                else
                {
                    context.Comments.Remove(element);
                    context.SaveChanges();
                }
            }
        }

        public List<CommentViewModel> Read(CommentBindingModel model)
        {
            using (var context = new DatabaseContext())
            {
                return context.Comments
                    .Where(rec => model == null || rec.Id == model.Id)
                    .Select(rec => new CommentViewModel
                    {
                        Id = rec.Id,
                        Header = rec.Header,
                        Text = rec.Text,
                        CreationDate = rec.CreationDate,
                        Author = rec.Author,
                        BlogId = rec.BlogId,
                        BlogName = rec.Blog.Name
                    })
                    .ToList();
            }
        }
    }
}
