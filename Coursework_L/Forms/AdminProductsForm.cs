using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coursework_L.Forms
{
    public partial class AdminProductsForm : Form
    {

        DataBase data = new DataBase("products", 6);

        public AdminProductsForm()
        {
            InitializeComponent();

            List<String[]> books = data.toList("SELECT `products`.`id`, `products`.`name`, " +
                "`products`.`coast`, `categories`.`name`, `manufactorers`.`name`, " +
                "`products`.`sales_count` " +
                "FROM `products` " +
                "JOIN `categories` ON `categories`.`id` = `products`.`category_id`" +
                "JOIN `manufactorers` ON `manufactorers`.`id` = `products`.`manufactorer_id`");
            fillDataGridView(books);
        }

        private void fillDataGridView(List<string[]> list)
        {
            foreach (string[] str in list) dataGridView1.Rows.Add(str);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[6].Value = "Редактировать";
                dataGridView1.Rows[i].Cells[6].Style.ForeColor = Color.Blue;
                dataGridView1.Rows[i].Cells[6].Style.Font = new Font(dataGridView1.Font, FontStyle.Underline);
                dataGridView1.Rows[i].Cells[7].Value = "Удалить";
                dataGridView1.Rows[i].Cells[7].Style.ForeColor = Color.Blue;
                dataGridView1.Rows[i].Cells[7].Style.Font = new Font(dataGridView1.Font, FontStyle.Underline);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            string orderBy = "id";

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    orderBy = "`name`";
                    break;
                case 1:
                    orderBy = "`coast` DESC";
                    break;
                case 2:
                    orderBy = "`category_id`";
                    break;
                case 3:
                    orderBy = "`manufactorer_id`";
                    break;
                case 4:
                    orderBy = "`sales_count` DESC";
                    break;
            }

            List<string[]> books = data.toList("SELECT `products`.`id`, `products`.`name`, " +
                "`products`.`coast`, `categories`.`name`, `manufactorers`.`name`, " +
                "`products`.`sales_count` " +
                "FROM `products` " +
                "JOIN `categories` ON `categories`.`id` = `products`.`category_id`" +
                "JOIN `manufactorers` ON `manufactorers`.`id` = `products`.`manufactorer_id` " +
                "WHERE `products`.`name` LIKE '%" + search + "%' " +
                "ORDER BY `products`." + orderBy);
            
            dataGridView1.Rows.Clear();
            fillDataGridView(books);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string orderBy = "id";
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    orderBy = "`name`";
                    break;
                case 1:
                    orderBy = "`coast` DESC";
                    break;
                case 2:
                    orderBy = "`category_id`";
                    break;
                case 3:
                    orderBy = "`manufactorer_id` DESC";
                    break;
                case 4:
                    orderBy = "`sales_count` DESC";
                    break;
            }

            List<string[]> books = data.toList("SELECT `products`.`id`, `products`.`name`, " +
                "`products`.`coast`, `categories`.`name`, `manufactorers`.`name`, " +
                "`products`.`sales_count` " +
                "FROM `products` " +
                "JOIN `categories` ON `categories`.`id` = `products`.`category_id`" +
                "JOIN `manufactorers` ON `manufactorers`.`id` = `products`.`manufactorer_id` " +
                "ORDER BY `products`." + orderBy);

            textBox1.Text = "";
            dataGridView1.Rows.Clear();
            fillDataGridView(books);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int editColumnNumber = 6;
            int deleteColumnNumber = 7;

            if (e.ColumnIndex == editColumnNumber)
            {
                DataGridViewRow currentRow = dataGridView1.Rows[e.RowIndex];

                int id = int.Parse(currentRow.Cells[0].Value.ToString());
                string name = currentRow.Cells[1].Value.ToString();
                string coast = currentRow.Cells[2].Value.ToString();
                string category = currentRow.Cells[3].Value.ToString();
                string manufactorer = currentRow.Cells[4].Value.ToString();
                string sales_count = currentRow.Cells[5].Value.ToString();

                Hide();
                AdminEditProductForm adminEditProductForm =
                    new AdminEditProductForm(id, name, coast, category, manufactorer, sales_count);
                adminEditProductForm.Show();
            }
            else if (e.ColumnIndex == deleteColumnNumber)
            {
                String caption = "Удаление книги из базы";
                String message = "Вы уверены, что хотите удалить товар?";
                DialogResult result = MessageBox.Show(message, caption,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataGridViewRow currentRow = dataGridView1.Rows[e.RowIndex];
                    data.deleteElement(int.Parse(currentRow.Cells[0].Value.ToString()));
                    dataGridView1.Rows.RemoveAt(currentRow.Index);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            AdminAddProductForm addProductForm = new AdminAddProductForm();
            addProductForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            AdminMainMenu adminMainMenu = new AdminMainMenu();
            adminMainMenu.Show();
        }

        private void AdminProductsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
