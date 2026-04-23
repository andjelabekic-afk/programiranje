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

namespace Andjela_DVDKolekcijaA13
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_A13_DVDKolekcija;Integrated Security=True"); /* MM 2 sp*/

        SqlCommand kom = new SqlCommand();

        SqlDataReader dr;

        int id = 0;

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void PuniGridChart()
        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniGridIChart", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            chart1.DataSource = dt;
            dataGridView1.DataSource = dt;

            chart1.Series["Series1"].XValueMember = "Producent";
            chart1.Series["Series1"].YValueMembers = "BrojFilmova";
            chart1.Titles.Add("DVD KOLEKCIJA");

            Kon.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PuniGridChart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
