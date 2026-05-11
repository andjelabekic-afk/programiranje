using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Andjela_ProduzeniBoravakA5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-2UBJC86\SQLEXPRESS;Initial Catalog=4EIT_A5_ProduzeniBoravak;Integrated Security=True"); /* MM 2 sp*/

        SqlCommand kom = new SqlCommand();

        SqlDataReader dr;

        int id = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            PuniKontroluLV();
        }

        private void PrazniKontrole()
        {
            textBox2.Clear();
            comboBox1.Text = "";
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
        }
        private void UnosAktivnosti()
        {
            Kon.Open();
            SqlCommand cmd = new SqlCommand("UnesiAktivnost", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NazivAktivnosti", SqlDbType.VarChar).Value = textBox1.Text.ToString();
            cmd.Parameters.AddWithValue("@Dan", SqlDbType.VarChar).Value = comboBox1.Text.ToString();
            cmd.Parameters.AddWithValue("@Pocetak", SqlDbType.Date).Value = maskedTextBox1.Text.ToString();
            cmd.Parameters.AddWithValue("@Zavrsetak", SqlDbType.Date).Value = maskedTextBox2.Text.ToString();

            cmd.ExecuteNonQuery();

            Kon.Close();

        }
        private void PuniKontroluLV()
        {
            listView1.Items.Clear();

            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniListView", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                ListViewItem red = new ListViewItem(dr[0].ToString());
                for (int i = 1; i < 5; i++) /* i IDE DO KOLIKO POLJA VRACA PROCEDURA*/
                    red.SubItems.Add(dr[i].ToString());
                listView1.Items.Add(red);
            }
            Kon.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UnosAktivnosti();
            PuniKontroluLV();
            PrazniKontrole();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
