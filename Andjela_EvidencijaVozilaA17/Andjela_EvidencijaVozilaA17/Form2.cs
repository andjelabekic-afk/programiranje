using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_EvidencijaVozilaA17
{
    public partial class Form2 : Form
    {
        SqlConnection Kon = new SqlConnection(
            @"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_A17_EvidencijaVozila;Integrated Security=True");

        public Form2()
        {
            InitializeComponent();
            this.Load += Form2_Load; // OVO JE BITNO! povezuje event sa metodom
        }

        // Load event
        private void Form2_Load(object sender, EventArgs e)
        {
            PuniComboBox();
        }

        // Popunjavanje ComboBox-a sa modelima
        private void PuniComboBox()
        {
            try
            {
                Kon.Open();
                SqlCommand cmd = new SqlCommand("PuniComboModel", Kon);
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Naziv";
                Kon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom učitavanja modela: " + ex.Message);
            }
        }

        // Dugme PRIKAŽI
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            string model = comboBox1.Text;

            try
            {
                Kon.Open();
                SqlCommand cmd = new SqlCommand("PuniGridChart", Kon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Model", model);

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                // Popunjavanje DataGridView
                dataGridView1.DataSource = dt;

                // Popunjavanje Chart
                chart1.Series.Clear();
                chart1.Series.Add("Prosečna cena");
                chart1.Series["Prosečna cena"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie; // PIE ovde
                chart1.Series["Prosečna cena"].XValueMember = "GodinaProizvodnje";
                chart1.Series["Prosečna cena"].YValueMembers = "ProsCena";
                chart1.DataSource = dt;
                chart1.DataBind();

                Kon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom učitavanja podataka: " + ex.Message);
            }
        }

        // Dugme IZAĐI
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}