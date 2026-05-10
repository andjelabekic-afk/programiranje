using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_evidencijaZaposlenihA19
{
    public partial class Form1 : Form
    {

        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-2UBJC86\SQLEXPRESS;Initial Catalog=4EIT_A19_EvidencijaZaposlenih;Integrated Security=True");
        SqlDataReader dr;

        public Form1()
        {
            InitializeComponent();
        }

        private void PuniComboSektor()
        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand("SELECT Naziv FROM Sektor", Kon);
            cmd.CommandType = CommandType.Text;

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Naziv";

            Kon.Close();
        }

        private void PuniPodatkeOSektoru()
        {
            if (Kon.State == ConnectionState.Closed)
                Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniPodatkeOSektoru", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SektorNaziv", comboBox1.Text);

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                richTextBox1.Text = comboBox1.Text + "\n" + dr[0].ToString();
            }

            Kon.Close();
        }

        private void PuniListView()
        {
            listView1.Items.Clear();

            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniListView", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SektorNaziv", comboBox1.Text);

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                ListViewItem red = new ListViewItem(dr[0].ToString());

                for (int i = 1; i < 5; i++)
                    red.SubItems.Add(dr[i].ToString());

                listView1.Items.Add(red);
            }

            Kon.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PuniPodatkeOSektoru();
            PuniListView();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            int sektorID = 0;
            Kon.Open();
            SqlCommand cmd = new SqlCommand("SELECT SektorID FROM Sektor WHERE Naziv=@Naziv", Kon);
            cmd.Parameters.AddWithValue("@Naziv", comboBox1.Text);
            sektorID = (int)cmd.ExecuteScalar();
            Kon.Close();

            Form2 f = new Form2(sektorID);
            f.ShowDialog();

            PuniListView(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PuniComboSektor();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}