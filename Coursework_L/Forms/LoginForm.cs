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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            
            LoginField.ForeColor = Color.Gray;
            PassField.ForeColor = Color.Gray;

            LoginField.Text = "Введите логин";
            PassField.Text = "Введите пароль";
        }

        private void LogIn(String login, String password)
        {
            DataBase data = new DataBase();

            MySqlCommand checkUserDataCommand = new MySqlCommand("SELECT COUNT(*)" +
                " FROM `users` WHERE `login` = @log_str" +
                " AND `password` = @pass_str", data.GetConnection());

            checkUserDataCommand.Parameters.Add("@log_str", MySqlDbType.VarChar).Value = login;
            checkUserDataCommand.Parameters.Add("@pass_str", MySqlDbType.VarChar).Value = password;

            data.openConnection();
            bool isCorrect = int.Parse(checkUserDataCommand.ExecuteScalar().ToString()) == 1;
            data.closeConnection();

            if (isCorrect)
            {
                Hide();

                List<string[]> userData = data.toList("SELECT `id`, `role` FROM `users` " +
                    "WHERE `login` = '" + login + "'", 2);

                data.openConnection();
                bool isAdmin = userData[0][1] == "True";
                data.closeConnection();

                if (isAdmin)
                {
                    AdminMainMenu adminMainMenu = new AdminMainMenu();
                    adminMainMenu.Show();
                    
                }
                else
                {
                    ProductsForm productsForm = new ProductsForm(int.Parse(userData[0][0]));
                    productsForm.Show();
                }
            }
            else
            {
                MessageBox.Show("Проверьте введённые данные", "Ошибка");
            }
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            string login = LoginField.Text;
            string password = PassField.Text;

            LogIn(login, password);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Hide();
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }

        private void LoginField_Enter(object sender, EventArgs e)
        {
            if (LoginField.Text == "Введите логин")
            {
                LoginField.ForeColor = Color.Black;
                LoginField.Text = "";
            }
        }

        private void LoginField_Leave(object sender, EventArgs e)
        {
            if (LoginField.Text == "")
            {
                LoginField.ForeColor = Color.Gray;
                LoginField.Text = "Введите логин";
            }

        }

        private void PassField_Enter(object sender, EventArgs e)
        {
            if (PassField.Text == "Введите пароль")
            {
                PassField.UseSystemPasswordChar = true;
                PassField.ForeColor = Color.Black;
                PassField.Text = "";
            }
        }

        private void PassField_Leave(object sender, EventArgs e)
        {
            if (PassField.Text == "")
            {
                PassField.UseSystemPasswordChar = false;
                PassField.ForeColor = Color.Gray;
                PassField.Text = "Введите пароль";
            }
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
