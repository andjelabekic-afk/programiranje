using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Andjela_FakultetskaEvidencijaA7
{
    public partial class Form3 : Form
    {
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-2UBJC86\SQLEXPRESS;Initial Catalog=4EIT_A7_FakultetskaEvidencija;Integrated Security=True");

        public Form3()
        {
            InitializeComponent();

            this.Load += Form3_Load;

            button1.Click += button1_Click; // Dugme Prikaži
            button2.Click += button2_Click; // Dugme Izađi
        }

        // Pri otvaranju forme puni CheckedListBox
        private void Form3_Load(object sender, EventArgs e)
        {
            PuniCheckListBox();
        }

        private void PuniCheckListBox()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("PuniCheckListBox", Kon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    Kon.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        checkedListBox1.Items.Clear();
                        while (reader.Read())
                        {
                            checkedListBox1.Items.Add(reader["Predmet"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju predmeta: " + ex.Message);
            }
            finally
            {
                if (Kon.State == ConnectionState.Open)
                    Kon.Close();
            }
        }

        // Dugme Prikaži: puni DataGridView i Chart
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Izaberite bar jedan predmet!");
                return;
            }

            if (checkedListBox1.CheckedItems.Count > 4) // Maks 4 predmeta
            {
                MessageBox.Show("Možete izabrati najviše 4 predmeta!");
                return;
            }

            string predmet1 = checkedListBox1.CheckedItems.Count > 0 ? checkedListBox1.CheckedItems[0].ToString() : null;
            string predmet2 = checkedListBox1.CheckedItems.Count > 1 ? checkedListBox1.CheckedItems[1].ToString() : null;
            string predmet3 = checkedListBox1.CheckedItems.Count > 2 ? checkedListBox1.CheckedItems[2].ToString() : null;
            string predmet4 = checkedListBox1.CheckedItems.Count > 3 ? checkedListBox1.CheckedItems[3].ToString() : null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("PuniGridChart", Kon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Predmet1", (object)predmet1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Predmet2", (object)predmet2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Predmet3", (object)predmet3 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Predmet4", (object)predmet4 ?? DBNull.Value);

                    Kon.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        //  DataGridView
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dataGridView1.DataSource = dt;
                        dataGridView1.AutoResizeColumns();

                        // Chart
                        chart1.Series.Clear();
                        chart1.ChartAreas.Clear();
                        ChartArea area = new ChartArea();
                        chart1.ChartAreas.Add(area);

                        // Grid linije horizontalne
                        area.AxisX.MajorGrid.LineColor = Color.LightGray;
                        area.AxisY.MajorGrid.LineColor = Color.LightGray;
                        area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                        area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

                        // Reset reader da ponovo čitamo iste podatke
                        reader.Close();
                        using (SqlCommand cmd2 = new SqlCommand("PuniGridChart", Kon))
                        {
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@Predmet1", (object)predmet1 ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@Predmet2", (object)predmet2 ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@Predmet3", (object)predmet3 ?? DBNull.Value);
                            cmd2.Parameters.AddWithValue("@Predmet4", (object)predmet4 ?? DBNull.Value);

                            using (SqlDataReader chartReader = cmd2.ExecuteReader())
                            {
                                while (chartReader.Read())
                                {
                                    string naziv = chartReader["Predmet"].ToString();
                                    Series s = new Series(naziv)
                                    {
                                        ChartType = SeriesChartType.Bar, 
                                        IsValueShownAsLabel = true
                                    };

                                    s.Points.AddXY("2020", chartReader["2020"]);
                                    s.Points.AddXY("2021", chartReader["2021"]);
                                    s.Points.AddXY("2022", chartReader["2022"]);

                                    chart1.Series.Add(s);
                                }
                            }
                        }
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
