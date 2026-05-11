using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_BibliotekaA2
{
    public partial class Form1 : Form
    {
        // Promeni samo naziv baze ako se kod tebe u SQL-u zove drugačije.
        // PAZI: ovde je latinicno A u EIT_A02_Biblioteka.
        private readonly string konekcija = @"DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=EIT_A02_Biblioteka;Integrated Security=True";

        SqlConnection Kon;
        SqlDataReader dr;
        int id = 0;

        public Form1()
        {
            InitializeComponent();
            Kon = new SqlConnection(konekcija);
        }

        private bool UDesigneru()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (UDesigneru()) return;

            try
            {
                textBox1.Enabled = false;
                PuniListViu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju podataka: " + ex.Message);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PuniListViu()
        {
            listView1.Items.Clear();

            try
            {
                if (Kon.State != ConnectionState.Open)
                    Kon.Open();

                SqlCommand cmd = new SqlCommand("PuniListView", Kon);
                cmd.CommandType = CommandType.StoredProcedure;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ListViewItem red = new ListViewItem(dr[0].ToString());
                    for (int i = 1; i < 4; i++)
                        red.SubItems.Add(dr[i].ToString());

                    listView1.Items.Add(red);
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                    dr.Close();

                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

        private void SaListViuNaKontrole()
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                id = Convert.ToInt32(item.SubItems[0].Text);

                textBox1.Text = id.ToString();
                textBox2.Text = item.SubItems[1].Text;
                textBox3.Text = item.SubItems[2].Text;
                maskedTextBox1.Text = item.SubItems[3].Text;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaListViuNaKontrole();
        }

        private void BrisiAutora()
        {
            try
            {
                if (Kon.State != ConnectionState.Open)
                    Kon.Open();

                SqlCommand cmd = new SqlCommand("BrisiAutor", Kon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AutorID", Convert.ToInt32(textBox1.Text.Trim()));

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string poruka = "Zelite da obrisete autora?";
            string naslov = "Brisanje autora";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(poruka, naslov, buttons);

            if (result == DialogResult.Yes)
            {
                try
                {
                    BrisiAutora();
                    PuniListViu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri brisanju: " + ex.Message);
                }
            }
        }
    }
}
