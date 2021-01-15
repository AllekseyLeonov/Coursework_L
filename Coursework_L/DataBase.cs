using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_L.Forms
{
    class DataBase
    {
        public DataBase() { }

        public DataBase(String name, int countOfElements)
        {
            this.name = name;
            countOfColumns = countOfElements;
        }

        String name;
        int countOfColumns;

        private MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;" +
            "username=root;password=root;database=comp_shop");

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }

        public List<String[]> toList(String MySQLCommand, int countOfElements)
        {
            openConnection();

            MySqlCommand command = new MySqlCommand(MySQLCommand, connection);
            MySqlDataReader reader = command.ExecuteReader();

            List<String[]> list = new List<string[]>();
            while (reader.Read())
            {
                list.Add(new String[countOfElements]);
                int last = list.Count - 1;
                for (int i = 0; i < countOfElements; i++)
                {
                    list[last][i] = reader[i].ToString();
                }
            }

            reader.Close();
            closeConnection();

            return list;
        }

        public List<String[]> toList(String MySQLCommand)
        {
            return toList(MySQLCommand, countOfColumns);
        }

        public void deleteElement(int id)
        {
            deleteElement(name, id);
        }

        public void deleteElement(string table_name, int id)
        {
            openConnection();
            String MySQLCommand = "DELETE FROM `" + table_name + "` WHERE `" + table_name + "`.`id` =" + id.ToString();
            MySqlCommand command = new MySqlCommand(MySQLCommand, connection);
            command.ExecuteNonQuery();
            closeConnection();
        }
    }
}
