using BlogBusinessLogic.Interfaces;
using BlogDatabaseImplementation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace BlogDatabaseImplementation.Implementations
{
    public class BackupLogic
    {
        private readonly IBlogLogic bLogic;
        private readonly ICommentLogic cLogic;
        public BackupLogic(IBlogLogic bLogic, ICommentLogic cLogic)
        {
            this.bLogic = bLogic;
            this.cLogic = cLogic;
        }
        public void CreateBackup(string folderName)
        {
            var files = Directory.GetFiles(folderName);
            if (files.Length != 0)
            {
                throw new Exception("Выбранная папка не пустая");
            }
            SaveToFile<Blog>(folderName);
            SaveToFile<Comment>(folderName);
        }
        private void SaveToFile<T>(string folderName) where T : class, new()
        {
            List<T> records;
            using (var context = new DatabaseContext())
            {
                records = context.Set<T>().ToList();
            }
            if (records != null)
            {
                var serializer = new DataContractJsonSerializer(typeof(List<T>));
                using (var fs = new FileStream(
                    $"{folderName}/{new T().GetType().Name}.json", 
                    FileMode.OpenOrCreate))
                {
                    serializer.WriteObject(fs, records);
                }
            }
        }

        [Obsolete]
        public void LoadBackup(string folderName)
        {
            var fileNames = Directory.GetFiles(folderName);
            if (fileNames.Length == 0)
            {
                throw new Exception("Выбранная папка пустая");
            }
            bool containsBlogs = false;
            bool containsComments = false;
            Regex blogRegex = new Regex(@"Blog[.]json$");
            Regex commentRegex = new Regex(@"Comment[.]json$");
            foreach (var fileName in fileNames)
            {
                if (blogRegex.IsMatch(fileName))
                {
                    containsBlogs = true;
                }
                if (commentRegex.IsMatch(fileName))
                {
                    containsComments = true;
                }
            }
            if (!(containsBlogs && containsComments))
            {
                throw new Exception("В папке отсутствуют некоторые файлы бэкапа");
            }
            using (var context = new DatabaseContext())
            {
                var blogs = GetFromFile<Blog>(folderName);
                var comments = GetFromFile<Comment>(folderName);
                context.Database.ExecuteSqlCommand("DELETE FROM Comments");
                context.Database.ExecuteSqlCommand("DELETE FROM Blogs");
                foreach (var blog in blogs)
                {
                    context.Blogs.Add(new Blog
                    {
                        Name = blog.Name,
                        Author = blog.Author,
                        CreationDate = blog.CreationDate
                    });
                }
                context.SaveChanges();
                foreach (var comment in comments)
                {
                    var blogNameToSearch = blogs.First(b => b.Id == comment.BlogId).Name;
                    context.Comments.Add(new Comment
                    {
                        Author = comment.Author,
                        BlogId = context.Blogs.First(rec => rec.Name == blogNameToSearch).Id,
                        CreationDate = comment.CreationDate,
                        Header = comment.Header,
                        Text = comment.Text
                    });
                }               
                context.SaveChanges();
            }
        }
        private List<T> GetFromFile<T>(string folderName) where T : class, new()
        {
            List<T> records;
            var serializer = new DataContractJsonSerializer(typeof(List<T>));
            using (var fs = new FileStream(
                    $"{folderName}/{new T().GetType().Name}.json",
                    FileMode.Open))
            {
                records = (List<T>)serializer.ReadObject(fs);
            }
            return records;
        }
    }
}
