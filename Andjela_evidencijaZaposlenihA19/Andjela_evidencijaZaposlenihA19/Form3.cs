using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_evidencijaZaposlenihA19
{
    public partial class Form3 : Form
    {

        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-2UBJC86\SQLEXPRESS;Initial Catalog=4EIT_A19_EvidencijaZaposlenih;Integrated Security=True");

        public Form3()
        {
            InitializeComponent();
        }

        private void PuniChart()
        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniChart", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            chart1.DataSource = dt;

            chart1.Series["Series1"].XValueMember = "Prezime";
            chart1.Series["Series1"].YValueMembers = "Dana";

            Kon.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            PuniChart();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}