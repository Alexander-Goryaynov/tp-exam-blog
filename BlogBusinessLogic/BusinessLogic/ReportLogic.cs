using BlogBusinessLogic.BindingModels;
using BlogBusinessLogic.Interfaces;
using BlogBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Security.Cryptography.X509Certificates;

namespace BlogBusinessLogic.BusinessLogic
{
    public class ReportLogic
    {
        private readonly IBlogLogic bLogic;
        private readonly ICommentLogic cLogic;
        public ReportLogic(IBlogLogic bLogic, ICommentLogic cLogic)
        {
            this.bLogic = bLogic;
            this.cLogic = cLogic;
        }
        public void SaveBlogCommentsToPdf(ReportBindingModel model)
        {
            using (var fs = new FileStream(model.FileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                Document doc = new Document();
                try
                {                    
                    //открываем файл для работы                
                    doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                    doc.Open();
                    BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf",
                                BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    //вставляем заголовок
                    var phraseTitle = new Phrase("Отчет",
                    new Font(baseFont, 16, Font.BOLD));
                    Paragraph paragraph = new Paragraph(phraseTitle)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 12
                    };
                    doc.Add(paragraph);
                    // вставляем даты
                    var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString() +
                        " по " + model.DateTo.Value.ToShortDateString(),
                        new Font(baseFont, 14, Font.BOLD));
                    paragraph = new Paragraph(phrasePeriod)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 12
                    };
                    doc.Add(paragraph);
                    //вставляем таблицу, задаем количество столбцов, и ширину колонок
                    PdfPTable table = new PdfPTable(5)
                    {
                        TotalWidth = 800F
                    };
                    table.SetTotalWidth(new float[] { 160, 100, 120, 180, 120 });
                    //вставляем шапку
                    PdfPCell cell = new PdfPCell();
                    var fontForCellBold = new Font(baseFont, 10, Font.BOLD);
                    table.AddCell(new PdfPCell(new Phrase("Название блога", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    table.AddCell(new PdfPCell(new Phrase("Дата создания блога", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    table.AddCell(new PdfPCell(new Phrase("Заголовок комментария", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    table.AddCell(new PdfPCell(new Phrase("Автор комментария", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    table.AddCell(new PdfPCell(new Phrase("Дата комментария", fontForCellBold))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                    //заполняем таблицу
                    var list = GetBlogComments(model);
                    var fontForCells = new Font(baseFont, 10);
                    foreach (var elem in list)
                    {
                        cell = new PdfPCell(new Phrase(elem.BlogName, fontForCells));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(elem.BlogCreationDate, fontForCells));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(elem.CommentHeader, fontForCells));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(elem.CommentAuthor, fontForCells));
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(elem.CommentCreationDate, fontForCells));
                        table.AddCell(cell);
                    }
                    doc.Add(table);
                }
                finally
                {
                    doc.Close();
                }
            }
        }
        private List<ReportBlogCommentsViewModel> GetBlogComments(ReportBindingModel model)
        {
            var result = new List<ReportBlogCommentsViewModel>();
            var blogs = bLogic.Read(null);
            // удаляем блоги, не подходящие по дате
            if (model.DateFrom.HasValue)
            {
                blogs.RemoveAll(rec => rec.CreationDate < model.DateFrom.Value);
            }
            if (model.DateTo.HasValue)
            {
                blogs.RemoveAll(rec => rec.CreationDate > model.DateTo.Value);
            }
            var comments = cLogic.Read(null);
            // удаляем комментарии, блоги которых были удалены из списка
            comments.RemoveAll(rec => (blogs.FirstOrDefault(b => b.Id == rec.BlogId)) == null);
            // сортировка для того, чтобы сгруппировать комментарии по блогам
            comments.Sort((x, y) => (x.BlogId.CompareTo(y.BlogId)));
            foreach(var comment in comments)
            {
                var record = new ReportBlogCommentsViewModel
                {
                    CommentAuthor = comment.Author,
                    CommentCreationDate = comment.CreationDate.ToString(),
                    CommentHeader = comment.Header
                };
                foreach (var blog in blogs)
                {
                    if (comment.BlogId == blog.Id)
                    {
                        // если комментарий не первый в блоге
                        if (result.Count > 0 && result.Last().BlogName.Equals(blog.Name))
                        {
                            record.BlogName = " ";
                            record.BlogCreationDate = " ";
                        } 
                        else
                        {
                            record.BlogName = blog.Name;
                            record.BlogCreationDate = blog.CreationDate.ToShortDateString();
                        }                        
                    }
                }
                result.Add(record);
            }
            return result;
        }
    }
}
