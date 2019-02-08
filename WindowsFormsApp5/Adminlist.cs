using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Adminlist : Form
    {
        SqlConnection connection = new SqlConnection(Properties.Settings.Default.dbConnectionString);
        public Adminlist()
        {
            InitializeComponent();
            connection.Open();
        }
       

        private void Adminlist_Load(object sender, EventArgs e)
        {
            refreshGrid();
        }
        private void refreshGrid()
        {

            string sql = "SELECT id,userid,date,type,counter,delivery,total,confirmed FROM orders";
          
            SqlCommand sqlcommand = new SqlCommand(sql, connection);
            SqlDataAdapter sqladapter = new SqlDataAdapter(sqlcommand);
            SqlCommandBuilder sqlbuilder = new SqlCommandBuilder(sqladapter);
            DataSet ds = new DataSet();
            sqladapter.Fill(ds, "orders");
            DataTable dt = ds.Tables["orders"];
            //  connection.Close();
            dataGridView1.DataSource = ds.Tables["orders"];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                label1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string sql = "SELECT login,name,address FROM users WHERE id = " +dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                SqlCommand sqlcommand = new SqlCommand(sql, connection);
                SqlDataReader sqlreader = sqlcommand.ExecuteReader();

                while (sqlreader.Read())
                {
                    label2.Text = Convert.ToString(sqlreader[0]);
                    label3.Text = Convert.ToString(sqlreader[1]);
                    label4.Text = Convert.ToString(sqlreader[2]);
                  
                }
                if (Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[7].Value) == 1)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                sqlreader.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
          

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            string sql = "";
            if (checkBox1.Checked)
            {
                sql = "UPDATE orders SET confirmed = 1 where id = " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString();



            }
            else
            {
                sql = "UPDATE orders SET confirmed = 0 where id = " + dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            }
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader rdr = cmd.ExecuteReader();
            rdr.Close();
            MessageBox.Show("Confirmation successful!");
            refreshGrid();

        }
    }
}
