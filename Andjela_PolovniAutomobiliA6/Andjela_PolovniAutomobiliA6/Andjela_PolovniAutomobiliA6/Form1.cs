using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace PolovniAutomobili
{
    public partial class Form1 : Form
    {
        SqlConnection Kon = new SqlConnection(
            @"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_А6_PolovniAutomobili;Integrated Security=True"
        );

        DataTable dtModeli;

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;

            
            button1.Click += button1_Click; 
            button2.Click += button2_Click;
            button3.Click += button3_Click; 
            listBox1.Click += listBox1_Click;
         


            
            button4.Click += button4_Click; 

           
            try { button5.Click += button3_Click; } catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
           
            try { PuniComboProizvodjac(); } catch { }

            PuniListBoxModeli();
            ResetLeviDeo();


           
        }


        private void PuniComboProizvodjac()
        {
            try
            {
                Kon.Open();
                SqlCommand cmd = new SqlCommand("dbo.PuniComboProizvodjac", Kon);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Naziv";
                comboBox1.ValueMember = "ProizvodjacID";
                comboBox1.SelectedIndex = -1;
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            finally
            {
                if (Kon.State == ConnectionState.Open) Kon.Close();
            }
        }

    

        private void PuniListBoxModeli()
        {
            try
            {
                Kon.Open();
                SqlCommand cmd = new SqlCommand("dbo.PuniListBoxModeli", Kon);
                cmd.CommandType = CommandType.StoredProcedure;

                dtModeli = new DataTable();
                dtModeli.Load(cmd.ExecuteReader());

                listBox1.DataSource = dtModeli;

                if (dtModeli.Columns.Contains("Prikaz"))
                    listBox1.DisplayMember = "Prikaz";
                else if (dtModeli.Columns.Contains("ModelAutomobila"))
                    listBox1.DisplayMember = "ModelAutomobila";
                else
                    listBox1.DisplayMember = dtModeli.Columns[0].ColumnName;

                
                if (dtModeli.Columns.Contains("ModelId"))
                    listBox1.ValueMember = "ModelId";

                listBox1.SelectedIndex = -1;
            }
            finally
            {
                if (Kon.State == ConnectionState.Open) Kon.Close();
            }
        }

     
        private void ResetLeviDeo()
        {
            listBox1.ClearSelected();
            listBox1.SelectedIndex = -1;

            try { comboBox1.SelectedIndex = -1; } catch { }
            textBox2.Text = "";
        }

     
        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

           
            if (listBox1.SelectedItem is DataRowView r && dtModeli != null)
            {
                if (dtModeli.Columns.Contains("ModelId") && dtModeli.Columns.Contains("ModelNaziv"))
                {
                    textBox1.Text = r["ModelId"].ToString();
                    textBox2.Text = r["ModelNaziv"].ToString();

                    if (dtModeli.Columns.Contains("ProizvodjacID"))
                        comboBox1.SelectedValue = Convert.ToInt32(r["ProizvodjacID"]);
                    else if (dtModeli.Columns.Contains("ProizvodjacNaziv"))
                        comboBox1.Text = r["ProizvodjacNaziv"].ToString();

                    return;
                }
            }

           
            string linija = listBox1.GetItemText(listBox1.SelectedItem);
            string[] delovi = linija.Split('-');
            if (delovi.Length < 3) return;

            textBox1.Text = delovi[0].Trim(); 
            textBox2.Text = delovi[1].Trim(); 
            comboBox1.Text = delovi[2].Trim(); 
        }

  
        private void button1_Click(object sender, EventArgs e)
        {
            string sifraTxt = textBox1.Text.Trim();

            if (sifraTxt == "")
            {
                ResetLeviDeo();
                return;
            }

            DataTable dt = new DataTable();

            try
            {
                Kon.Open();
                SqlCommand cmd = new SqlCommand("dbo.PuniListBoxUslov", Kon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ModelID", SqlDbType.NVarChar, 50).Value = sifraTxt;

                dt.Load(cmd.ExecuteReader());
            }
            finally
            {
                if (Kon.State == ConnectionState.Open) Kon.Close();
            }

            if (dt.Rows.Count == 0)
            {
                ResetLeviDeo();
                MessageBox.Show("Model sa unetom šifrom ne postoji.");
                return;
            }

         
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string s = listBox1.GetItemText(listBox1.Items[i]).TrimStart();
                if (s.StartsWith(sifraTxt))
                {
                    listBox1.SelectedIndex = i;
                    return;
                }
            }

            ResetLeviDeo();
        }

  
        private void button2_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text.Trim(), out int modelId))
            {
                MessageBox.Show("Unesi ispravnu šifru (broj).");
                return;
            }

            string naziv = textBox2.Text.Trim();
            if (naziv == "")
            {
                MessageBox.Show("Unesi naziv.");
                return;
            }

            try
            {
                Kon.Open();
                SqlCommand cmd = new SqlCommand("dbo.IzmeniModel", Kon);
                cmd.CommandType = CommandType.StoredProcedure;

               
                cmd.Parameters.Add("@ModelNaziv", SqlDbType.NVarChar, 50).Value = naziv;
                cmd.Parameters.Add("@ModelID", SqlDbType.Int).Value = modelId;

              
                if (comboBox1.SelectedValue != null)
                {
                   
                    try
                    {
                        cmd.Parameters.Add("@ProizvodjacID", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
                    }
                    catch { }
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Izmena nije uspela: " + ex.Message);
                return;
            }
            finally
            {
                if (Kon.State == ConnectionState.Open) Kon.Close();
            }

            MessageBox.Show("Uspešno izmenjeno.");

            PuniListBoxModeli();
            textBox1.Text = modelId.ToString();
            button1.PerformClick();
        }

    
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

      
        private void button4_Click(object sender, EventArgs e)
        {
            PuniListBoxIChart();
        }

        private void PuniListBoxIChart()
        {
            int godOd = (int)numericUpDown1.Value;
            int godDo = (int)numericUpDown2.Value;

            if (!int.TryParse(textBox3.Text.Trim(), out int km))
            {
                MessageBox.Show("Kilometraža mora biti broj.");
                return;
            }

            DataTable dt = new DataTable();

            try
            {
                Kon.Open();

                SqlCommand cmd = new SqlCommand("dbo.PuniGridChart", Kon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@GodisteOD", SqlDbType.Int).Value = godOd;
                cmd.Parameters.Add("@GodisteDO", SqlDbType.Int).Value = godDo;
                cmd.Parameters.Add("@Kilometraza", SqlDbType.Int).Value = km;

                dt.Load(cmd.ExecuteReader());
            }
            finally
            {
                if (Kon.State == ConnectionState.Open) Kon.Close();
            }

            
            listBox2.Items.Clear();
            foreach (DataRow r in dt.Rows)
            {
                string proizvodjac = r["Proizvodjac"].ToString();
                string br = r["BrVozila"].ToString();
                listBox2.Items.Add(proizvodjac.PadRight(20) + br);
            }

            
            chart1.Series["Series1"].Points.Clear();
            chart1.Series["Series1"].ChartType = SeriesChartType.Column;
            chart1.ChartAreas[0].AxisX.Interval = 1;

            chart1.DataSource = dt;
            chart1.Titles.Clear();
            chart1.Titles.Add($"Godište {godOd}-{godDo}, KM <= {km}");

            chart1.Series["Series1"].XValueMember = "Proizvodjac";
            chart1.Series["Series1"].YValueMembers = "BrVozila";

            chart1.DataBind();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
