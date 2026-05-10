using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_EvidencijaKnjigaA11
{
    public partial class Form1 : Form
    {
        SqlConnection Kon = new SqlConnection(
    @"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_A11_EvidencijaKnjiga;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false; // UPIŠI
            button3.Enabled = false; // ODUSTANI
            textBox1.Enabled = true;

            PuniListView();
            PrikaziPrvogAutora();
        }

        private void PuniListView()
        {
            listView1.Items.Clear();
            listView1.View = View.Details;

            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("Šifra");
                listView1.Columns.Add("Ime");
                listView1.Columns.Add("Prezime");
                listView1.Columns.Add("Datum rođenja");
            }

            Kon.Open();
            SqlCommand cmd = new SqlCommand("PuniListView", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr["AutorID"].ToString());
                item.SubItems.Add(dr["Ime"].ToString());
                item.SubItems.Add(dr["PrezimeIme"].ToString());
                item.SubItems.Add(dr["DatumRodjenja"].ToString());
                listView1.Items.Add(item);
            }
            Kon.Close();
        }

        private void PrikaziPrvogAutora()
        {
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                SaListViewNaKontrole();
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            SaListViewNaKontrole();
        }

        private void SaListViewNaKontrole()
        {
            if (listView1.SelectedItems.Count == 0) return;

            ListViewItem item = listView1.SelectedItems[0];
            textBox1.Text = item.SubItems[0].Text;
            textBox2.Text = item.SubItems[1].Text;
            textBox3.Text = item.SubItems[2].Text;
            dateTimePicker1.Value = Convert.ToDateTime(item.SubItems[3].Text);
        }

        // RESET
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            textBox1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        // UPIŠI
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Unesite ime i prezime autora!");
                return;
            }

            Kon.Open();
            SqlCommand cmd = new SqlCommand("InsertAutor", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ImeAutora", textBox2.Text);
            cmd.Parameters.AddWithValue("@PrezimeAutora", textBox3.Text);
            cmd.Parameters.AddWithValue("@DatumRodjena", dateTimePicker1.Value);

            cmd.ExecuteNonQuery();
            Kon.Close();

            MessageBox.Show("Autor je uspešno upisan!");

            textBox1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;

            PuniListView();
        }

        // ODUSTANI
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
               "Aplikacija služi za evidenciju autora i analizu broja knjiga po autorima.",
               "O aplikaciji");
        }

    
    }
}
