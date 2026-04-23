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

namespace Andjela_RibolovackoDrustvoA10
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_A10_RibolovackoDrustvo;Integrated Security=True"); /* MM 2 sp*/

        SqlCommand kom = new SqlCommand();

        SqlDataReader dr;

        int id = 0;
        private void Form2_Load(object sender, EventArgs e)
        {
            PuniComboPecaros();
        }

        private void PrikazGridaICharta()
        {
            string Par_Pecaros = comboBox1.Text.ToString();
            string[] PPecaros = Par_Pecaros.Split('-');


            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniGridiChart", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PecarosID", SqlDbType.Int).Value = Convert.ToInt32(PPecaros[0].Trim());
            cmd.Parameters.Add("@DatumOD", SqlDbType.Date).Value = dateTimePicker1.Value.Date;
            cmd.Parameters.Add("@DatumDO", SqlDbType.Date).Value = dateTimePicker2.Value.Date;
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            chart1.DataSource = dt;
            dataGridView1.DataSource = dt;

            chart1.Series["Series1"].XValueMember = "Naziv";
            chart1.Series["Series1"].YValueMembers = "Broj";
            chart1.Titles.Add("RIBE");

            Kon.Close();
        }
        private void PuniComboPecaros()

        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniComboPecaros", Kon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Pecaros";


            Kon.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            chart1.Titles.Clear();

            PrikazGridaICharta();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}