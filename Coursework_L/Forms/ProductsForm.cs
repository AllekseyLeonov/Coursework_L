using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Coursework_L.Forms
{
    public partial class ProductsForm : Form
    {
        DataBase data = new DataBase("products", 5);
        List<string> products_to_order = new List<string>();
        int user_id;

        public ProductsForm(int user_id)
        {
            InitializeComponent();
            this.user_id = user_id;
            List<string[]> products = data.toList("SELECT `products`.`id`, `products`.`name`, " +
                "`products`.`coast`, `categories`.`name`, `manufactorers`.`name` " +
                "FROM `products` " +
                "JOIN `categories` ON `categories`.`id` = `products`.`category_id`" +
                "JOIN `manufactorers` ON `manufactorers`.`id` = `products`.`manufactorer_id`");
            fillDataGridView(products);
        }

        private void fillDataGridView(List<string[]> list)
        {
            foreach (String[] str in list) dataGridView1.Rows.Add(str);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[5].Value = "Добавить в корзину";
                dataGridView1.Rows[i].Cells[5].Style.ForeColor = Color.Blue;
                dataGridView1.Rows[i].Cells[5].Style.Font = new Font(dataGridView1.Font, FontStyle.Underline);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 5)
            {
                products_to_order.Add(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                MessageBox.Show("Продукт был добавлен в корзину", "Подтверждение");
            }
        }

        private void ProductsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            BasketForm basket = new BasketForm(products_to_order, user_id);
            basket.Show();
        }
    }
}
