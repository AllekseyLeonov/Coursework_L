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
    public partial class AdminUsersForm : Form
    {
        DataBase data = new DataBase("users", 4);
        List<string[]> users;

        public AdminUsersForm()
        {
            InitializeComponent();
            users = data.toList("SELECT `users`.`id`, `users`.`name`, `users`.`role`" +
                " FROM `users` ORDER BY `id`", 3);
            fillDataGridView(users);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int listOfBooksColumnNumber = 3;
            int editColumnNumber = 4;
            int deleteColumnNumber = 5;

            DataGridViewRow currentRow = dataGridView1.Rows[e.RowIndex];
            int id = int.Parse(currentRow.Cells[0].Value.ToString());

            if (e.ColumnIndex == editColumnNumber)
            {
                string name = currentRow.Cells[1].Value.ToString();
                string role = currentRow.Cells[2].Value.ToString();

                Hide();
                AdminEditUserForm adminEditReaderForm = new AdminEditUserForm(id, name, role);
                adminEditReaderForm.Show();
            }
            else if (e.ColumnIndex == deleteColumnNumber)
            {
                string caption = "Удаление пользователя из базы";
                string message = "Вы уверены, что хотите удалить пользователя?";
                DialogResult result = MessageBox.Show(message, caption,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    data.deleteElement(id);
                    dataGridView1.Rows.RemoveAt(currentRow.Index);
                }
            }
            else if (e.ColumnIndex == listOfBooksColumnNumber)
            {
                string cell_value = currentRow.Cells[e.ColumnIndex].Value.ToString();
                if (cell_value != "Заказов нет")
                {
                    Hide();
                    AdminOrdersForm list =
                        new AdminOrdersForm(currentRow.Cells[0].Value.ToString());
                    list.Show();
                }
            }
        }

        private void fillDataGridView(List<String[]> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                dataGridView1.Rows.Add();
                DataGridViewRow currentRow = dataGridView1.Rows[i];
                currentRow.Cells[0].Value = list[i][0];
                currentRow.Cells[1].Value = list[i][1];
                if (list[i][2] == "False") currentRow.Cells[2].Value = "Пользователь";
                else currentRow.Cells[2].Value = "Администратор";
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                int countOfOrders = getCountOfOrders(row.Cells[0].Value.ToString());
                if (countOfOrders > 0)
                {
                    row.Cells[3].Value = "Просмотреть список";
                    makeLinkStyle(row.Cells[3]);
                }
                else row.Cells[3].Value = "Заказов нет";

                row.Cells[4].Value = "Редактировать";
                makeLinkStyle(row.Cells[4]);
                row.Cells[5].Value = "Удалить";
                makeLinkStyle(row.Cells[5]);
            }
        }

        private int getCountOfOrders(string userID)
        {
            int count = 0;
            data.openConnection();

            MySqlCommand mySqlCommand = new MySqlCommand("SELECT COUNT(*)" +
                " FROM `orders` WHERE `user_id` = " + userID, data.GetConnection());
            count = int.Parse(mySqlCommand.ExecuteScalar().ToString());
            data.closeConnection();
            return count;
        }

        private void makeLinkStyle(DataGridViewCell cell)
        {
            cell.Style.ForeColor = Color.Blue;
            cell.Style.Font = new Font(dataGridView1.Font, FontStyle.Underline);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            AdminMainMenu adminMainMenu = new AdminMainMenu();
            adminMainMenu.Show();
        }

        private void AdminUsersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
