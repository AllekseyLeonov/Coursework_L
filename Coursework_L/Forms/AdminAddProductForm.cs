using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Coursework_L.Forms
{
    public partial class AdminAddProductForm : Form
    {
        string name;
        float coast;
        int category_id;
        int manufactorer_id;
        DataBase data = new DataBase();
        List<string[]> categories;
        List<string[]> manufactorers;

        public AdminAddProductForm()
        {
            InitializeComponent();

            categories = data.toList("SELECT * FROM `categories`", 2);
            manufactorers = data.toList("SELECT * FROM `manufactorers`", 2);

            foreach (string[] category in categories) comboBox1.Items.Add(category[1]);
            foreach (string[] manufactorer in manufactorers) comboBox2.Items.Add(manufactorer[1]);

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            if(name.Length > 30)
                MessageBox.Show("Имя слишком длинное", "Ошибка");
            else if (name.Length == 0)
                MessageBox.Show("Имя слишком короткое", "Ошибка");
            else if (!isFloatFormate(textBox2.Text))
                MessageBox.Show("Введите цену в формате ХХ,ХХ", "Ошибка");
            else
            {
                coast = float.Parse(textBox2.Text);
                for (int i = 0; i < comboBox1.Items.Count; i++)
                    if (categories[i][1] == comboBox1.SelectedItem.ToString())
                        category_id = int.Parse(categories[i][0]);
                for (int i = 0; i < comboBox2.Items.Count; i++)
                    if (manufactorers[i][1] == comboBox2.SelectedItem.ToString())
                        manufactorer_id = int.Parse(manufactorers[i][0]);

                string command = "INSERT INTO `products`" +
                        " (`id`, `name`, `coast`, `category_id`, `manufactorer_id`, `sales_count`)" +
                        " VALUES (NULL, @name, @coast, @category_id, @manufactorer_id, @sales_count);";
                MySqlCommand mySqlCommand = new MySqlCommand(command, data.GetConnection());

                mySqlCommand.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                mySqlCommand.Parameters.Add("@coast", MySqlDbType.Float).Value = coast;
                mySqlCommand.Parameters.Add("@category_id", MySqlDbType.Int32).Value = category_id;
                mySqlCommand.Parameters.Add("@manufactorer_id", MySqlDbType.Int32).Value = manufactorer_id;
                mySqlCommand.Parameters.Add("@sales_count", MySqlDbType.Int32).Value = 0;

                data.openConnection();
                mySqlCommand.ExecuteNonQuery();
                data.closeConnection();

                Hide();
                AdminProductsForm adminProductsForm= new AdminProductsForm();
                adminProductsForm.Show();
            }
        }

        private bool isFloatFormate(string str)
        {
            return Regex.IsMatch(str, @"[0-9]{2}\,[0-9]{2}");
        }

        private void AddProductForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            AdminProductsForm adminProductsForm = new AdminProductsForm();
            adminProductsForm.Show();
        }
    }
}
