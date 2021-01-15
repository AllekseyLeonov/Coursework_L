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
    public partial class AdminEditProductForm : Form
    {
        DataBase data = new DataBase();
        List<string[]> categories;
        List<string[]> manufactorers;
        int id;

        public AdminEditProductForm(int id, string name, string coast, string category,
            string manufactorer, string sales_count)
        {
            InitializeComponent();

            this.id = id;

            categories = data.toList("SELECT * FROM `categories`", 2);
            manufactorers = data.toList("SELECT * FROM `manufactorers`", 2);

            foreach (string[] str in categories) comboBox1.Items.Add(str[1]);
            foreach (string[] str in manufactorers) comboBox2.Items.Add(str[1]);

            textBox1.Text = name;
            textBox2.Text = coast;
            comboBox1.SelectedItem = category;
            comboBox2.SelectedItem = manufactorer;
            textBox3.Text = sales_count;
        }

        private bool isFloatFormate(string str)
        {
            return Regex.IsMatch(str, @"[0-9]{2}\,[0-9]{2}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            float coast;
            int category_id = 0;
            int manufactorer_id = 0;

            if (name.Length > 30)
                MessageBox.Show("Имя слишком длинное", "Ошибка");
            if (name.Length == 0)
                MessageBox.Show("Имя слишком короткое", "Ошибка");
            else if (!isFloatFormate(textBox2.Text))
                MessageBox.Show("Введите цену в формате ХХ,ХХ", "Ошибка");
            else if (!int.TryParse(textBox3.Text, out int sales_count))
                MessageBox.Show("Введённое количество некорректно", "Ошибка");
            else
            {
                coast = float.Parse(textBox2.Text);
                for (int i = 0; i < comboBox1.Items.Count; i++)
                    if (categories[i][1] == comboBox1.SelectedItem.ToString())
                        category_id = int.Parse(categories[i][0]);
                for (int i = 0; i < comboBox2.Items.Count; i++)
                    if (manufactorers[i][1] == comboBox2.SelectedItem.ToString())
                        manufactorer_id = int.Parse(manufactorers[i][0]);

                string command = "UPDATE `products` SET" +
                        " `name` = @name, `coast` = @coast, `category_id` = @category_id," +
                        " `manufactorer_id` = @manufactorer_id, `sales_count` = @sales_count " +
                        "WHERE `products`.`id` = @id";
                MySqlCommand mySqlCommand = new MySqlCommand(command, data.GetConnection());

                mySqlCommand.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                mySqlCommand.Parameters.Add("@coast", MySqlDbType.Float).Value = coast;
                mySqlCommand.Parameters.Add("@category_id", MySqlDbType.Int32).Value = category_id;
                mySqlCommand.Parameters.Add("@manufactorer_id", MySqlDbType.Int32).Value = manufactorer_id;
                mySqlCommand.Parameters.Add("@sales_count", MySqlDbType.Int32).Value = sales_count;
                mySqlCommand.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                data.openConnection();
                mySqlCommand.ExecuteNonQuery();
                data.closeConnection();

                Hide();
                AdminProductsForm adminProductsForm = new AdminProductsForm();
                adminProductsForm.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            AdminProductsForm adminProductsForm = new AdminProductsForm();
            adminProductsForm.Show();
        }
    }
}
