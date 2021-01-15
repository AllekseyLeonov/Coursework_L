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
    public partial class ManufactorerStatisticsForm : Form
    {
        public ManufactorerStatisticsForm()
        {
            InitializeComponent();
            DataBase data = new DataBase();
            List<string[]> manufactorers = data.toList("SELECT `manufactorers`.`name`," +
                " `products`.`sales_count`, `products`.`coast` FROM `manufactorers` " +
                "JOIN `products` ON `products`.`manufactorer_id` = `manufactorers`.`id` " +
                "ORDER BY `products`.`sales_count`", 3);
            for (int i = 0; i < manufactorers.Count; i++)
            {
                dataGridView1.Rows.Add();
                double count = double.Parse(manufactorers[i][1]);
                double coast = double.Parse(manufactorers[i][2]);
                dataGridView1.Rows[i].Cells[0].Value = manufactorers[i][0];
                dataGridView1.Rows[i].Cells[1].Value = Convert.ToString(coast * count);
            }
        }
    }
}
