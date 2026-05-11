using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_BibliotekaA2
{
    public partial class Form2 : Form
    {
        // Promeni samo naziv baze ako se kod tebe u SQL-u zove drugačije.
        // PAZI: ovde je latinicno A u EIT_A02_Biblioteka.
        private readonly string konekcija = @"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=EIT_A02_Biblioteka;Integrated Security=True";

        SqlConnection Kon;

        public Form2()
        {
            InitializeComponent();
            Kon = new SqlConnection(konekcija);
        }

        private bool UDesigneru()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (UDesigneru()) return;

            try
            {
                PuniComboAutor();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju autora: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PuniGridChart()
        {
            string Par_Autor = comboBox1.Text.ToString();
            string[] PAutor = Par_Autor.Split('-');

            try
            {
                if (Kon.State != ConnectionState.Open)
                    Kon.Open();

                SqlCommand cmd = new SqlCommand("PuniGridChart", Kon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SifraAutora", Convert.ToInt32(PAutor[0].Trim()));
                cmd.Parameters.AddWithValue("@Period", Convert.ToInt32(numericUpDown1.Value));

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                chart1.DataSource = dt;
                dataGridView1.DataSource = dt;

                chart1.Series["Series1"].XValueMember = "GodUzimanja";
                chart1.Series["Series1"].YValueMembers = "BrIznajmljivanja";
                chart1.DataBind();
                chart1.Titles.Add("AUTOR");
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

        private void PuniComboAutor()
        {
            try
            {
                if (Kon.State != ConnectionState.Open)
                    Kon.Open();

                SqlCommand cmd = new SqlCommand("PuniComboAutor", Kon);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Autor";
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                chart1.Titles.Clear();
                PuniGridChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri prikazu analize: " + ex.Message);
            }
        }
    }
}
