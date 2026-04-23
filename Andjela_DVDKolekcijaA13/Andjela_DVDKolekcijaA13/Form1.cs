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

namespace Andjela_DVDKolekcijaA13
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=4EIT_A13_DVDKolekcija;Integrated Security=True"); /* MM 2 sp*/

        SqlCommand kom = new SqlCommand();

        SqlDataReader dr;

        int id = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.FullRowSelect = true;
            PuniLV();
        }

        private void PuniLV()
        {
            listView1.Items.Clear();

            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniListView", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                ListViewItem red = new ListViewItem(dr[0].ToString());
                for (int i = 1; i < 3; i++) /* i IDE DO KOLIKO POLJA VRACA PROCEDURA*/
                    red.SubItems.Add(dr[i].ToString());
                listView1.Items.Add(red);
            }
            Kon.Close();
        }
        private void SaLV_NaKontrole()
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                id = Convert.ToInt32(item.SubItems[0].Text);

                textBox1.Text = id.ToString();
                textBox2.Text = item.SubItems[1].Text;
                textBox3.Text = item.SubItems[2].Text;

            }
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            SaLV_NaKontrole();
        }

        private void Izmena()
        {
            if (!int.TryParse(textBox1.Text.Trim(), out int producentId))
            {
                MessageBox.Show("ProducentID mora biti broj (selektuj red u listi).");
                return;
            }

            using (SqlCommand cmd = new SqlCommand("IzmeniUpdate", Kon))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ProducentID", SqlDbType.Int).Value = producentId;
                cmd.Parameters.Add("@Ime", SqlDbType.NVarChar, 50).Value = textBox2.Text.Trim();
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = textBox3.Text.Trim();

                Kon.Open();
                cmd.ExecuteNonQuery();
                Kon.Close();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Izmena();
            PuniLV();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }


        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
