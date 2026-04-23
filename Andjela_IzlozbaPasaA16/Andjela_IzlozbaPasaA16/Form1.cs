using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Andjela_IzlozbaPasaA16
{
    public partial class Form1 : Form
    {
        private readonly SqlConnection Kon = new SqlConnection(
            @"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_A16_IzlozbaPasa;Integrated Security=True"
        );

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;

            button1.Click += button1_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;
            button4.Click += button4_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;

            richTextBox1.Clear();
            richTextBox1.Text = "O aplikaciji\r\n\r\n" +
                "Aplikacija \"Izložba pasa\" namenjena je vođenju evidencije o prijavama pasa na izložbe i prikazu statističkih podataka o učesnicima. " +
                "Omogućava unos nove prijave izborom psa, izložbe i kategorije, uz proveru da isti pas nije već prijavljen na izabranu izložbu. " +
                "Takođe, aplikacija omogućava tabelarni i grafički prikaz broja pasa po kategorijama za izabranu održanu izložbu, kao i prikaz ukupnog broja prijavljenih pasa i broja pasa koji su se takmičili.\r\n\r\n" +
                "Aplikacija je povezana sa bazom podataka, a svi podaci se čuvaju i obrađuju u skladu sa zahtevima zadatka. " +
                "Korisnički interfejs je organizovan kroz tabove radi lakšeg korišćenja i preglednosti. " +
                "Namenjena je jednostavnom i efikasnom radu korisnika pri evidenciji i analizi podataka o izložbama pasa.";

            PuniComboPas();
            PuniComboIzlozbaZaPrijavu();
            PuniComboKategorija();
            PuniComboIzlozbaZaStatistiku();

            chart1.Series.Clear();
            chart1.Titles.Clear();
        }

        private void PuniComboPas()
        {
            Kon.Open();
            using (SqlCommand cmd = new SqlCommand("PuniComboPas", Kon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Pas";
                comboBox1.SelectedIndex = -1;
            }
            Kon.Close();
        }

        private void PuniComboIzlozbaZaPrijavu()
        {
            Kon.Open();
            using (SqlCommand cmd = new SqlCommand("PuniComboIzlozba", Kon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "Izlozba";
                comboBox2.SelectedIndex = -1;
            }
            Kon.Close();
        }

        private void PuniComboKategorija()
        {
            Kon.Open();
            using (SqlCommand cmd = new SqlCommand("PuniComboKategorija", Kon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                comboBox3.DataSource = dt;
                comboBox3.DisplayMember = "Kategorija";
                comboBox3.SelectedIndex = -1;
            }
            Kon.Close();
        }

        private void PuniComboIzlozbaZaStatistiku()
        {
            Kon.Open();
            using (SqlCommand cmd = new SqlCommand("PuniComboIzlozba", Kon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                comboBox4.DataSource = dt;
                comboBox4.DisplayMember = "Izlozba";
                comboBox4.SelectedIndex = -1;
            }
            Kon.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Popuni sva polja (Pas, Izložba, Kategorija).");
                return;
            }

            int pasID = IzvuciIntID(comboBox1.Text);
            string izlozbaID = IzvuciIzlozbaID(comboBox2.Text);
            int kategorijaID = IzvuciIntID(comboBox3.Text);

            Kon.Open();

            using (SqlCommand provera = new SqlCommand(
                "SELECT COUNT(*) FROM Rezultat WHERE IzlozbaID=@IzlozbaID AND PasID=@PasID", Kon))
            {
                provera.Parameters.Add("@IzlozbaID", SqlDbType.NChar, 10).Value = izlozbaID;
                provera.Parameters.Add("@PasID", SqlDbType.Int).Value = pasID;

                int postoji = Convert.ToInt32(provera.ExecuteScalar());

                if (postoji > 0)
                {
                    Kon.Close();
                    MessageBox.Show("Pas je već prijavljen na izabranu izložbu!");
                    return;
                }
            }

            using (SqlCommand cmd = new SqlCommand("PrijavaPasa", Kon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IzlozbaID", SqlDbType.NChar, 10).Value = izlozbaID;
                cmd.Parameters.Add("@KategorijaID", SqlDbType.Int).Value = kategorijaID;
                cmd.Parameters.Add("@PasID", SqlDbType.Int).Value = pasID;

                cmd.ExecuteNonQuery();
            }

            Kon.Close();
            MessageBox.Show("Uspešan unos!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == -1)
            {
                MessageBox.Show("Izaberi izložbu.");
                return;
            }

            string izlozbaID = IzvuciIzlozbaID(comboBox4.Text);

            DataTable dt = VratiTabelu_PuniGridIChart(izlozbaID);
            dataGridView1.DataSource = dt;

            chart1.Series.Clear();
            chart1.Titles.Clear();

            Series s = chart1.Series.Add("Kategorije");
            s.ChartType = SeriesChartType.Pie;
            s.XValueMember = "Naziv";
            s.YValueMembers = "BrojPasa";

            chart1.DataSource = dt;
            chart1.Titles.Add(comboBox4.Text);
            chart1.DataBind();

            int ukupnoPrijavljenih = VratiSkalarniBroj("UkupanBrPrijavljenih", izlozbaID);
            int ukupnoTakmicio = VratiSkalarniBroj("UkupanBrKojiSeTakmicio", izlozbaID);

            label9.Text = ukupnoPrijavljenih.ToString();
            label10.Text = ukupnoTakmicio.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tabControl1 != null)
                tabControl1.SelectedIndex = 0;
        }

        private int IzvuciIntID(string tekst)
        {
            return int.Parse(tekst.Split('-')[0].Trim());
        }

        private string IzvuciIzlozbaID(string tekst)
        {
            return tekst.Split('-')[0].Trim();
        }

        private DataTable VratiTabelu_PuniGridIChart(string izlozbaID)
        {
            Kon.Open();
            using (SqlCommand cmd = new SqlCommand("PuniGridIChart", Kon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IzlozbaID", SqlDbType.NChar, 10).Value = izlozbaID;

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                Kon.Close();
                return dt;
            }
        }

        private int VratiSkalarniBroj(string spName, string izlozbaID)
        {
            Kon.Open();
            using (SqlCommand cmd = new SqlCommand(spName, Kon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IzlozbaID", SqlDbType.NChar, 10).Value = izlozbaID;

                object o = cmd.ExecuteScalar();
                Kon.Close();

                if (o == null || o == DBNull.Value) return 0;
                return Convert.ToInt32(o);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "Izlaz")
            {
                Application.Exit();
            }
        }
    }
}