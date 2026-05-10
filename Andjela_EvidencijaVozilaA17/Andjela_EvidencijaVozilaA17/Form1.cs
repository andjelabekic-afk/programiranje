using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_EvidencijaVozilaA17
{
    public partial class Form1 : Form
    {
        SqlConnection Kon = new SqlConnection(
        @"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_A17_EvidencijaVozila;Integrated Security=True");

        int id;

        public Form1()
        {
            InitializeComponent();
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            PuniListBox();
            PuniComboBox();
            button2.Visible = false;
        }

        

        private void PuniListBox()
        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniListBoxView", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            listBox1.DataSource = dt;
            listBox1.DisplayMember = "Registracija";
            listBox1.ValueMember = "VoziloID";

            Kon.Close();
        }

       

        private void PuniComboBox()
        {
            DataTable dt = new DataTable();

            
            Kon.Open();
            SqlCommand cmd = new SqlCommand("PuniComboModel", Kon);
            cmd.CommandType = CommandType.StoredProcedure;
            dt.Load(cmd.ExecuteReader());
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Naziv";
            Kon.Close();

           
            dt = new DataTable();
            Kon.Open();
            cmd = new SqlCommand("PuniComboBoja", Kon);
            cmd.CommandType = CommandType.StoredProcedure;
            dt.Load(cmd.ExecuteReader());
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Naziv";
            Kon.Close();

           
            dt = new DataTable();
            Kon.Open();
            cmd = new SqlCommand("PuniComboGorivo", Kon);
            cmd.CommandType = CommandType.StoredProcedure;
            dt.Load(cmd.ExecuteReader());
            comboBox3.DataSource = dt;
            comboBox3.DisplayMember = "Naziv";
            Kon.Close();
        }

      

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            DataRowView drv = listBox1.SelectedItem as DataRowView;

            if (drv == null)
                return;

            id = Convert.ToInt32(drv["VoziloID"]);

            textBox1.Text = drv["VoziloID"].ToString();
            textBox2.Text = drv["Registracija"].ToString();
            textBox3.Text = drv["GodinaProizvodnje"].ToString();
            textBox4.Text = drv["PredjenoKM"].ToString();
            textBox5.Text = drv["Cena"].ToString();
            comboBox1.Text = drv["Model"].ToString();
            comboBox2.Text = drv["Boja"].ToString();
            comboBox3.Text = drv["Gorivo"].ToString();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
            button2.Visible = true;
        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";

            textBox1.ReadOnly = false;
            button2.Visible = false;
        }

     

        private void IzmeniVozilo()
        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand("AzurirajPodatkeVozilo", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Registracija", textBox2.Text);
            cmd.Parameters.AddWithValue("@GodinaProizvodnje", textBox3.Text);
            cmd.Parameters.AddWithValue("@Kilometraza", textBox4.Text);
            cmd.Parameters.AddWithValue("@ModelNaziv", comboBox1.Text);
            cmd.Parameters.AddWithValue("@BojaNaziv", comboBox2.Text);
            cmd.Parameters.AddWithValue("@GorivoNaziv", comboBox3.Text);
            cmd.Parameters.AddWithValue("@Cena", textBox5.Text);
            cmd.Parameters.AddWithValue("@VoziloID", textBox1.Text);

            cmd.ExecuteNonQuery();
            Kon.Close();

            MessageBox.Show("Podaci su uspešno izmenjeni!");

            PuniListBox();
        }

    

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            IzmeniVozilo();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Evidencija vozila\nAuto plac PREVOZ\nAutor: Andjela Bekić");
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}