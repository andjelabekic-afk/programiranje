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

namespace Andjela_EvidencijaRadnikaA3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-2UBJC86\SQLEXPRESS;Initial Catalog=EIT_A03_EvidencijaRadnika;Integrated Security=True"); /* MM 2 sp*/

        SqlCommand kom = new SqlCommand();

        SqlDataReader dr;

        int id = 0;

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void PopuniChartGrid()
        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniGridChart", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Period", SqlDbType.Int).Value = numericUpDown1.Value;

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            chart1.DataSource = dt;
            dataGridView1.DataSource = dt;

            chart1.Series["Series1"].XValueMember = "BrojProjekata";
            chart1.Series["Series1"].YValueMembers = "BrRadnika";
            chart1.Titles.Add("EVIDENCIJA RADNIKA");

            Kon.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            chart1.Titles.Clear();
            PopuniChartGrid();
        }
    }
}
