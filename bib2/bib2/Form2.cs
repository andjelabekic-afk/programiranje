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

namespace bib2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=EIT_А02_Biblioteka;Integrated Security=True"); /* MM 2 sp*/

        SqlCommand kom = new SqlCommand();

        SqlDataReader dr;

        int id = 0;

        private void Form2_Load(object sender, EventArgs e)
        {
            PuniComboAutor();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
   

        private void PuniGridChart()
        {

            string Par_Autor = comboBox1.Text.ToString();
            string[] PAutor = Par_Autor.Split('-');

            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniGridChart", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SifraAutora", SqlDbType.Int).Value = Convert.ToInt32(PAutor[0].ToString().Trim());
            cmd.Parameters.AddWithValue("@Period", SqlDbType.Int).Value = numericUpDown1.Value;

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            chart1.DataSource = dt;
            dataGridView1.DataSource = dt;

            chart1.Series["Series1"].XValueMember = "GodUzimanja";
            chart1.Series["Series1"].YValueMembers = "BrIznajmljivanja";
            chart1.Titles.Add("AUTOR");

            Kon.Close();
        }

        private void PuniComboAutor()
        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniComboAutor", Kon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Autor";


            Kon.Close();
        }

       

        private void button1_Click_1(object sender, EventArgs e)
        {
            chart1.Titles.Clear();
            PuniGridChart();
        }

    
    }
}
