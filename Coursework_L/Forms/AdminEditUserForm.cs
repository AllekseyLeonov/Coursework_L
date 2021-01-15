using MySql.Data.MySqlClient;
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

namespace Coursework_L.Forms
{
    public partial class AdminEditUserForm : Form
    {
        int id;

        public AdminEditUserForm(int id, string name, string role)
        {
            InitializeComponent();

            this.id = id;
            textBox1.Text = name;
            if (role == "Администратор") checkBox1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isContainsIncorrectSymbols(textBox1.Text) || isContainsNumeral(textBox1.Text))
            {
                DataBase data = new DataBase();
                string command = "UPDATE `users` SET `name` = @name, `role` = @role" +
                    " WHERE `id` = @id";
                MySqlCommand mySqlCommand = new MySqlCommand(command, data.GetConnection());

                mySqlCommand.Parameters.Add("@name", MySqlDbType.VarChar).Value = textBox1.Text;

                if (checkBox1.Checked) mySqlCommand.Parameters.Add("@role", MySqlDbType.Int32).Value = 1;
                else mySqlCommand.Parameters.Add("@role", MySqlDbType.Int32).Value = 0;

                mySqlCommand.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                data.openConnection();
                mySqlCommand.ExecuteNonQuery();
                data.closeConnection();

                Hide();
                AdminUsersForm adminUsersForm = new AdminUsersForm();
                adminUsersForm.Show();
            }
            else MessageBox.Show("Имя содержит некорректные символы", "Отказ");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            AdminUsersForm adminUsersForm = new AdminUsersForm();
            adminUsersForm.Show();
        }

        private void AdminEditUserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private bool isContainsIncorrectSymbols(string str)
        {
            return Regex.IsMatch(str.Replace(" ", ""), @"\W");
        }

        private bool isContainsNumeral(string str)
        {
            string pattern = @"\d";
            return Regex.IsMatch(str, pattern);
        }
    }
}
