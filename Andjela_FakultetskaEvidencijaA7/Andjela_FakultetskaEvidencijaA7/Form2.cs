using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_FakultetskaEvidencijaA7
{
    public partial class Form2 : Form
    {
     
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-2UBJC86\SQLEXPRESS;Initial Catalog=4EIT_A7_FakultetskaEvidencija;Integrated Security=True");

        public Form2()
        {
            InitializeComponent();

            
            this.Load += Form2_Load;

           
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

           

          
            dataGridView1.CellClick += dataGridView1_CellClick;

           
            button1.Click += button1_Click; 
            button2.Click += button2_Click; 
        }

        
        private void Form2_Load(object sender, EventArgs e)
        {
            PuniGrid();
        }

      
        private void PuniGrid()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("PuniGrid", Kon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable dt = new DataTable();

                    Kon.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju podataka: " + ex.Message);
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

       
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells[0].Value?.ToString() ?? ""; // PredmetID
                textBox2.Text = row.Cells[1].Value?.ToString() ?? ""; // SifraPredmeta
                textBox3.Text = row.Cells[2].Value?.ToString() ?? ""; // Predmet
                textBox4.Text = row.Cells[3].Value?.ToString() ?? ""; // Semestar
                richTextBox1.Text = row.Cells[4].Value?.ToString() ?? ""; // Opis
            }
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Niste selektovali predmet za brisanje!");
                return;
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand("BrisiPredmet", Kon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PredmetID", Convert.ToInt32(textBox1.Text));

                    Kon.Open();
                    int rows = cmd.ExecuteNonQuery();
                    Kon.Close();

                    if (rows > 0)
                    {
                        MessageBox.Show("Predmet uspešno obrisan!");
                        PuniGrid();
                    }
                    else
                    {
                        MessageBox.Show("Brisanje nije uspelo.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška: " + ex.Message);
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }


        
        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }
    }
}
