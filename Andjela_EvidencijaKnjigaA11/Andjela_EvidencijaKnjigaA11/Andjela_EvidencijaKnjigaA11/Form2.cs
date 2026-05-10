using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_EvidencijaKnjigaA11
{
    public partial class Form2 : Form
    {
        SqlConnection Kon = new SqlConnection(
    @"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_A11_EvidencijaKnjiga;Integrated Security=True");
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                PuniCheckListBox();

                chart1.Series.Clear();
                chart1.Series.Add("Broj knjiga");
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom učitavanja forme: " + ex.Message);
            }
        }

        private void PuniCheckListBox()
        {
            try
            {
                checkedListBox1.Items.Clear();

                using (SqlCommand cmd = new SqlCommand("PuniCheckListBox", Kon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    Kon.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            checkedListBox1.Items.Add(dr["Autor"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri punjenju liste autora: " + ex.Message);
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

        // PRIKAŽI ANALIZU
        private void button3_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count != 3)
            {
                MessageBox.Show("Morate izabrati tačno 3 autora!");
                return;
            }

            try
            {
                chart1.Series[0].Points.Clear();

                using (SqlCommand cmd = new SqlCommand("PuniChart", Kon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Autor1", checkedListBox1.CheckedItems[0].ToString());
                    cmd.Parameters.AddWithValue("@Autor2", checkedListBox1.CheckedItems[1].ToString());
                    cmd.Parameters.AddWithValue("@Autor3", checkedListBox1.CheckedItems[2].ToString());

                    Kon.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            chart1.Series[0].Points.AddXY(
                                dr["Autor"].ToString(),
                                Convert.ToInt32(dr["BrKnjiga"])
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri prikazu grafikona: " + ex.Message);
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

        // RESET
        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        // ZATVORI
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
