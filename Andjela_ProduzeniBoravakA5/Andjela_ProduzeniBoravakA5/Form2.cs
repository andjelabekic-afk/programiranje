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

namespace Andjela_ProduzeniBoravakA5
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-2UBJC86\SQLEXPRESS;Initial Catalog=4EIT_A5_ProduzeniBoravak;Integrated Security=True"); /* MM 2 sp*/

        SqlCommand kom = new SqlCommand();

        SqlDataReader dr;
        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void GrafickiNumerickiPrikaz()

        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniGridChart", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            chart1.DataSource = dt;
            dataGridView1.DataSource = dt;

            chart1.Series["Series1"].XValueMember = "Dan";
            chart1.Series["Series1"].YValueMembers = "BrDece";
            chart1.Titles.Add("DNEVNE AKTIVNOSTI");

            Kon.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Titles.Clear();
            GrafickiNumerickiPrikaz();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
