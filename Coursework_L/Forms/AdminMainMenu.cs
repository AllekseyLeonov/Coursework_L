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
    public partial class AdminMainMenu : Form
    {
        public AdminMainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            AdminProductsForm adminBooksForm = new AdminProductsForm();
            adminBooksForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            AdminUsersForm adminUsersForm = new AdminUsersForm();
            adminUsersForm.Show();
        }

        private void AdminMainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ManufactorerStatisticsForm manufactorerStatistics = new ManufactorerStatisticsForm();
            manufactorerStatistics.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CategoryStatisticsForm categoryStatistics = new CategoryStatisticsForm();
            categoryStatistics.Show();
        }
    }
}
