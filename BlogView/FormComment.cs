using BlogBusinessLogic.BindingModels;
using BlogBusinessLogic.Interfaces;
using BlogBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace BlogView
{
    public partial class FormComment : Form
    {
        public new IUnityContainer Container { get; set; }
        private readonly ICommentLogic cLogic;
        private readonly IBlogLogic bLogic;
        public int Id { set { id = value; } }
        private int? id;
        public FormComment(ICommentLogic cLogic, IBlogLogic bLogic)
        {
            InitializeComponent();
            this.cLogic = cLogic;
            this.bLogic = bLogic;
        }

        private void FormComment_Load(object sender, EventArgs e)
        {
            var items = bLogic.Read(null);
            if (items != null)
            {
                comboBoxBlog.DisplayMember = "Name";
                comboBoxBlog.ValueMember = "Id";
                comboBoxBlog.DataSource = items;
                if (!id.HasValue)
                {
                    comboBoxBlog.SelectedItem = null;
                } 
                else
                {
                    var boxBlogId = cLogic.Read(new CommentBindingModel { Id = id })[0].BlogId;
                    foreach (var item in comboBoxBlog.Items)
                    {
                        if((item as BlogViewModel).Id == boxBlogId)
                        {
                            comboBoxBlog.SelectedItem = item;
                        }
                    }
                }                
            }
            if (id.HasValue)
            {
                try
                {
                    var view = cLogic.Read(new CommentBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        textBoxHeader.Text = view.Header;
                        textBoxText.Text = view.Text;
                        textBoxAuthor.Text = view.Author;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxHeader.Text) ||
                string.IsNullOrEmpty(textBoxText.Text) ||
                string.IsNullOrEmpty(textBoxAuthor.Text) ||
                comboBoxBlog.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var dateTime = (id.HasValue) ? cLogic.Read(
                    new CommentBindingModel { Id = id })?[0].CreationDate : DateTime.Now;
                cLogic?.CreateOrUpdate(new CommentBindingModel
                {
                    Id = id,
                    Header = textBoxHeader.Text,
                    Text = textBoxText.Text,
                    Author = textBoxAuthor.Text,
                    BlogId = (comboBoxBlog.SelectedItem as BlogViewModel).Id,
                    CreationDate = (DateTime)dateTime
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
