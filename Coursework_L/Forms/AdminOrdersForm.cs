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
    public partial class AdminOrdersForm : Form
    {
        DataBase data = new DataBase();
        List<string[]> orders;
        public AdminOrdersForm(string user_id)
        {
            InitializeComponent();
            orders = data.toList("SELECT `orders`.`id` FROM `orders` " +
                "WHERE `orders`.`user_id` = " + user_id, 1);
            foreach (string[] order in orders) listBox1.Items.Add("Заказ номер " + order[0]);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hide();
            AdminPersonOrderForm adminPersonOrderForm =
                new AdminPersonOrderForm(orders[listBox1.SelectedIndex][0]);
            adminPersonOrderForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            AdminUsersForm adminUsersForm = new AdminUsersForm();
            adminUsersForm.Show();
        }

        private void AdminOrdersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
