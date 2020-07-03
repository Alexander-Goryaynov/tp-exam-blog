using BlogBusinessLogic.BindingModels;
using BlogBusinessLogic.Interfaces;
using System;
using System.Windows.Forms;
using Unity;

namespace BlogView
{
    public partial class FormComments : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly ICommentLogic logic;
        public FormComments(ICommentLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void блогиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormBlogs>();
            form.ShowDialog();
            LoadData();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormComment>();
            form.ShowDialog();
            LoadData();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormComment>();
                form.Id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить комментарий?", "Вопрос", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        logic.Delete(new CommentBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void FormComments_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = logic.Read(null);
                if (list != null)
                {
                    dataGridView.DataSource = list;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;                    
                    dataGridView.Columns[5].Visible = false;
                    dataGridView.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
