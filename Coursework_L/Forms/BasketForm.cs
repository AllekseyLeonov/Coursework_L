using MySql.Data.MySqlClient;
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
    public partial class BasketForm : Form
    {
        DataBase data = new DataBase();
        int user_id;
        List<string[]> products;
        List<string[]> methods;

        public BasketForm(List<string> products_id_list, int user_id)
        {
            InitializeComponent();
            this.user_id = user_id;
            products = new List<string[]>();
            methods = data.toList("SELECT * FROM `del_meth`", 2);
            foreach (string[] category in methods) comboBox1.Items.Add(category[1]);
            foreach (string product_id in products_id_list)
                products.Add(data.toList("SELECT `products`.`name`,`products`.`id` " +
                    "FROM `products` " +
                    "WHERE `id` = " + product_id, 2)[0]);
            foreach (string[] product in products) checkedListBox1.Items.Add(product[0]);
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, true);
            comboBox1.SelectedIndex = 0;
        }

        private void BasketForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool isAnyProductSelected = false;

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                if (checkedListBox1.GetItemChecked(i)) isAnyProductSelected = true;

            if (isAnyProductSelected)
            {
                data.openConnection();
                string command = "INSERT INTO `orders` (`id`, `user_id`, `date`, `del_meth_id`, `contacts`)" +
                    " VALUES (NULL, @user_id, @date, @del_meth_id, @contacts)";
                MySqlCommand mySqlCommand = new MySqlCommand(command, data.GetConnection());
                mySqlCommand.Parameters.Add("@user_id", MySqlDbType.Int32).Value = user_id;
                mySqlCommand.Parameters.Add("@date", MySqlDbType.VarChar).Value
                    = DateTime.Today.ToShortDateString();
                mySqlCommand.Parameters.Add("@del_meth_id", MySqlDbType.Int32).Value =
                    int.Parse(methods[comboBox1.SelectedIndex][0]);
                mySqlCommand.Parameters.Add("@contacts", MySqlDbType.VarChar).Value = textBox1.Text;
                mySqlCommand.ExecuteNonQuery();

                command = "SELECT MAX(`id`) FROM `orders`";
                mySqlCommand = new MySqlCommand(command, data.GetConnection());
                string order_id = mySqlCommand.ExecuteScalar().ToString();

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        isAnyProductSelected = true;
                        command = "INSERT INTO `prods_to_ords` (`id`, `product_id`, `order_id`)" +
                            " VALUES (NULL, @product_id, @order_id)";
                        mySqlCommand = new MySqlCommand(command, data.GetConnection());
                        mySqlCommand.Parameters.Add("@product_id", MySqlDbType.Int32).Value =
                            products[i][1];
                        mySqlCommand.Parameters.Add("@order_id", MySqlDbType.Int32).Value = int.Parse(order_id);
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
                data.closeConnection();
                Hide();
                ProductsForm productsForm = new ProductsForm(user_id);
                productsForm.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            ProductsForm products = new ProductsForm(user_id);
            products.Show();
        }
    }
}
