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
    public partial class RegisterForm : Form
    {
        DataBase data = new DataBase();
        string nameDefault = "Введите ваши ФИО";
        string loginDefault = "Введите логин";
        string passwordDefault = "Введите пароль";

        public RegisterForm()
        {
            InitializeComponent();

            PassField.AutoSize = false;
            PassField.Size = new Size(PassField.Width,
                LoginField.Height);

            nameField.ForeColor = Color.Gray;
            LoginField.ForeColor = Color.Gray;
            PassField.ForeColor = Color.Gray;

            nameField.Text = nameDefault;
            LoginField.Text = loginDefault;
            PassField.Text = passwordDefault;
        }

        private void addUserToDB(string login, string password, string name)
        {
            MySqlCommand addUserCommand = new MySqlCommand("INSERT INTO `users`" +
                " (`id`, `login`, `password`, `name`, `role`)" +
                " VALUES (NULL, @login, @password, @name , '0');", data.GetConnection());

            addUserCommand.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
            addUserCommand.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;
            addUserCommand.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;

            data.openConnection();
            addUserCommand.ExecuteNonQuery();
            data.closeConnection();
        }

        private Boolean IsLoginFree(String login)
        {
            MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM `users`" +
                " WHERE `login` = @log_str", data.GetConnection());
            command.Parameters.Add("@log_str", MySqlDbType.VarChar).Value = login;

            data.openConnection();
            int count = int.Parse(command.ExecuteScalar().ToString());
            data.closeConnection();
            if (count == 0) return true;
            else return false;
        }

        private Boolean IsUserDataOk(string name, string login, string password)
        {
            if (name == nameDefault)
            {
                MessageBox.Show(nameDefault, "Отказ");
                return false;
            }
            if (name.Length > 50)
            {
                MessageBox.Show("Имя слишком длинное", "Отказ");
                return false;
            }
            if (isContainsIncorrectSymbols(name) || isContainsNumeral(name))
            {
                MessageBox.Show("Имя содержит некорректные символы", "Отказ");
                return false;
            }
            if (login == loginDefault)
            {
                MessageBox.Show(loginDefault, "Отказ");
                return false;
            }
            if (password == passwordDefault)
            {
                MessageBox.Show(passwordDefault, "Отказ");
                return false;
            }
            if (!IsLoginFree(login))
            {
                MessageBox.Show("Логин уже занят");
                return false;
            }
            return true;
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

        

        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void register_button_Click_1(object sender, EventArgs e)
        {
            string login = LoginField.Text;
            string password = PassField.Text;
            string name = nameField.Text;

            if (IsUserDataOk(name, login, password))
            {
                addUserToDB(login, password, name);
                Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }

        private void nameField_Enter(object sender, EventArgs e)
        {
            if (nameField.Text == nameDefault)
            {
                nameField.ForeColor = Color.Black;
                nameField.Text = "";
            }
        }

        private void nameField_Leave(object sender, EventArgs e)
        {
            if (nameField.Text == "")
            {
                nameField.ForeColor = Color.Gray;
                nameField.Text = nameDefault;
            }
        }

        private void LoginField_Enter(object sender, EventArgs e)
        {
            if (LoginField.Text == loginDefault)
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
                LoginField.Text = loginDefault;
            }
        }

        private void PassField_Enter(object sender, EventArgs e)
        {
            if (PassField.Text == passwordDefault)
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
                PassField.Text = passwordDefault;
            }
        }

        private void RegisterForm_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
