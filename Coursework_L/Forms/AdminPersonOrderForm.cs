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
    public partial class AdminPersonOrderForm : Form
    {
        DataBase data = new DataBase();
        List<string[]> products;
        string order_id;

        public AdminPersonOrderForm(string order_id)
        {
            InitializeComponent();
            this.order_id = order_id;
            products = data.toList("SELECT `products`.`name`,`prods_to_ords`.`id`, `products`.`id` " +
                "FROM `products` " +
                "JOIN `prods_to_ords` ON `prods_to_ords`.`order_id` = " + order_id +
                " AND `prods_to_ords`.`product_id` = `products`.`id`", 3);
            foreach(string[] product in products) checkedListBox1.Items.Add(product[0]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++) checkedListBox1.SetItemChecked(i, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            data.openConnection();
            bool isAllElementsChecked = true;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                {
                    string mySQLCommand = "SELECT `sales_count` " +
                        "FROM `products` WHERE `id` = " + products[i][2];
                    MySqlCommand command = new MySqlCommand(mySQLCommand, data.GetConnection());

                    int count = int.Parse(command.ExecuteScalar().ToString());
                    count++;

                    mySQLCommand = "UPDATE `products` SET `sales_count` = '" + count.ToString() +
                        "' WHERE `products`.`id` = " + products[i][2];
                    command = new MySqlCommand(mySQLCommand, data.GetConnection());
                    command.ExecuteNonQuery();

                    data.deleteElement("prods_to_ords", int.Parse(products[i][1]));
                }
                else isAllElementsChecked = false;
            }
            if (isAllElementsChecked) data.deleteElement("orders", int.Parse(order_id));

            data.closeConnection();

            Hide();
            AdminUsersForm adminUsersForm = new AdminUsersForm();
            adminUsersForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            data.deleteElement("orders", int.Parse(order_id));
            
            Hide();
            AdminUsersForm adminUsersForm = new AdminUsersForm();
            adminUsersForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Hide();
            AdminUsersForm adminUsersForm = new AdminUsersForm();
            adminUsersForm.Show();
        }

        private void AdminPersonOrderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
