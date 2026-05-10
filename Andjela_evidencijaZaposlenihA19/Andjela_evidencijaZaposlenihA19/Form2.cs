using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Andjela_evidencijaZaposlenihA19
{
    public partial class Form2 : Form
    {
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-2UBJC86\SQLEXPRESS;Initial Catalog=4EIT_A19_EvidencijaZaposlenih;Integrated Security=True");
        private int sektorID;

        public Form2(int sektorID)
        {
            InitializeComponent();
            this.sektorID = sektorID; 
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            PuniCombo();
        }

   
        private void PuniCombo()
        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT CAST(RadnikID AS nvarchar) + '-' + Ime + ' ' + Prezime AS Radnik " +
                "FROM Radnik WHERE SektorID=@SektorID ORDER BY RadnikID", Kon);
            cmd.Parameters.AddWithValue("@SektorID", sektorID);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Radnik";

            Kon.Close();
        }

       
        private void ZatvoriStarog(DateTime datum)
        {
            Kon.Open();

            SqlCommand cmd = new SqlCommand(
                "UPDATE RukovodiSektorom SET DatumRazresenja=@DatumPostavljanja " +
                "WHERE SektorID=@SektorID AND DatumRazresenja IS NULL", Kon);
            cmd.Parameters.AddWithValue("@DatumPostavljanja", datum);
            cmd.Parameters.AddWithValue("@SektorID", sektorID);

            cmd.ExecuteNonQuery();
            Kon.Close();
        }

       
        private void NoviRukovodioc(DateTime datum)
        {
            string[] radnik = comboBox1.Text.Split('-');
            int radnikID = int.Parse(radnik[0]);

            Kon.Open();

            SqlCommand cmd = new SqlCommand(
                "INSERT INTO RukovodiSektorom (SektorID, RadnikID, DatumPostavljanja) " +
                "VALUES (@SektorID, @RadnikID, @DatumPostavljanja)", Kon);

            cmd.Parameters.AddWithValue("@SektorID", sektorID);
            cmd.Parameters.AddWithValue("@RadnikID", radnikID);
            cmd.Parameters.AddWithValue("@DatumPostavljanja", datum);

            cmd.ExecuteNonQuery();
            Kon.Close();
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
           
            DateTime datum;
            if (!DateTime.TryParse(textBox1.Text, out datum))
            {
                MessageBox.Show("Unesite validan datum (dd.mm.yyyy)!");
                return;
            }

           
            DateTime? aktuelniRazresenje = null;
            Kon.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT DatumRazresenja FROM RukovodiSektorom WHERE SektorID=@SektorID AND DatumRazresenja IS NULL", Kon);
            cmd.Parameters.AddWithValue("@SektorID", sektorID);
            var result = cmd.ExecuteScalar();
            if (result != DBNull.Value && result != null)
                aktuelniRazresenje = (DateTime)result;
            Kon.Close();

            
            if (aktuelniRazresenje.HasValue && datum <= aktuelniRazresenje.Value)
            {
                MessageBox.Show($"Datum imenovanja mora biti posle prethodnog datuma razrešenja: {aktuelniRazresenje.Value.ToShortDateString()}");
                return;
            }

           g
            ZatvoriStarog(datum);
            NoviRukovodioc(datum);

            MessageBox.Show("Uspešno dodat novi rukovodilac!");
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}