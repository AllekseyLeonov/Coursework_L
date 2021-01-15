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
    public partial class CategoryStatisticsForm : Form
    {
        public CategoryStatisticsForm()
        {
            InitializeComponent();
            DataBase data = new DataBase();
            List<string[]> categories = data.toList("SELECT `categories`.`name`," +
                " `products`.`sales_count`, `products`.`coast` FROM `categories` " +
                "JOIN `products` ON `products`.`category_id` = `categories`.`id` " +
                "ORDER BY `products`.`sales_count`", 3);
            for(int i = 0; i<categories.Count; i++)
            {
                dataGridView1.Rows.Add();
                double count = double.Parse(categories[i][1]);
                double coast = double.Parse(categories[i][2]);
                dataGridView1.Rows[i].Cells[0].Value = categories[i][0];
                dataGridView1.Rows[i].Cells[1].Value = Convert.ToString(coast * count);
            }
        }
    }
}
