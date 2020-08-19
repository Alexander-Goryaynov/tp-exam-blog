using BlogBusinessLogic.BindingModels;
using BlogBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace BlogView
{
    public partial class FormBlog : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private int? id;
        private readonly IBlogLogic logic;
        public FormBlog(IBlogLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormBlog_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new BlogBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.Name;
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
            if (string.IsNullOrEmpty(textBoxName.Text) ||
                string.IsNullOrEmpty(textBoxAuthor.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var regex = new Regex(@"\d");
            if (regex.IsMatch(textBoxAuthor.Text))
            {
                MessageBox.Show("Имя автора не может содержать цифр", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var dateTime = (id.HasValue) ? logic.Read(
                    new BlogBindingModel { Id = id })?[0].CreationDate : DateTime.Now;
                logic?.CreateOrUpdate(new BlogBindingModel
                {
                    Id = id,
                    Name = textBoxName.Text,
                    Author = textBoxAuthor.Text,
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
