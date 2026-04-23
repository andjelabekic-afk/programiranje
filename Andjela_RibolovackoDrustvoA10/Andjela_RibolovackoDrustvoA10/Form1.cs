using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_RibolovackoDrustvoA10
{
    public partial class Form1 : Form
    {
        private readonly SqlConnection Kon =
            new SqlConnection(@"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_A10_RibolovackoDrustvo;Integrated Security=True");

        private SqlDataReader dr;
        private int id = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            listView1.FullRowSelect = true;

            
            toolStripButton1.Click += toolStripButton1_Click; 
            toolStripButton2.Click += toolStripButton2_Click; 
            toolStripButton3.Click += toolStripButton3_Click; 
            toolStripButton4.Click += toolStripButton4_Click; 

            PunimComboGradovi();
            PunimListV();
        }

        

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
            IzmenaOsobe();
            PunimListV();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
          
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        // ===================== BAZA =====================

        private void IzmenaOsobe()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Prvo izaberi osobu iz liste.");
                return;
            }

            try
            {
                Kon.Open();

                using (SqlCommand cmd = new SqlCommand("IzmeniUpdatePecaros", Kon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PecarosID", SqlDbType.Int).Value =
                        Convert.ToInt32(textBox1.Text.Trim());

                    cmd.Parameters.Add("@Ime", SqlDbType.VarChar, 50).Value = textBox2.Text.Trim();
                    cmd.Parameters.Add("@Prezime", SqlDbType.VarChar, 50).Value = textBox3.Text.Trim();
                    cmd.Parameters.Add("@Adresa", SqlDbType.VarChar, 100).Value = textBox4.Text.Trim();
                    cmd.Parameters.Add("@Grad", SqlDbType.VarChar, 50).Value = comboBox1.Text.Trim();
                    cmd.Parameters.Add("@Telefon", SqlDbType.VarChar, 30).Value = textBox6.Text.Trim();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri izmeni: " + ex.Message);
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

        private void PunimComboGradovi()
        {
            try
            {
                Kon.Open();

                using (SqlCommand cmd = new SqlCommand("PuniComboGrad", Kon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        comboBox1.DataSource = dt;
                        comboBox1.DisplayMember = "Grad";
                        // Ako imaš ID grada u tabeli, možeš:
                        // comboBox1.ValueMember = "GradID";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri punjenju gradova: " + ex.Message);
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

        private void PunimListV()
        {
            listView1.Items.Clear();

            try
            {
                Kon.Open();

                using (SqlCommand cmd = new SqlCommand("PuniListView", Kon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ListViewItem red = new ListViewItem(dr[0].ToString());
                        for (int i = 1; i < 6; i++)
                            red.SubItems.Add(dr[i].ToString());

                        listView1.Items.Add(red);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri punjenju liste: " + ex.Message);
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                if (Kon.State == ConnectionState.Open) Kon.Close();
            }
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaListV_NaKontrole();
        }

        private void SaListV_NaKontrole()
        {
            if (listView1.SelectedItems.Count == 0) return;

            ListViewItem item = listView1.SelectedItems[0];

            id = Convert.ToInt32(item.SubItems[0].Text);

            textBox1.Text = id.ToString();
            textBox2.Text = item.SubItems[1].Text;
            textBox3.Text = item.SubItems[2].Text;
            textBox4.Text = item.SubItems[3].Text;
            comboBox1.Text = item.SubItems[4].Text;
            textBox6.Text = item.SubItems[5].Text;
        }
    }
}
