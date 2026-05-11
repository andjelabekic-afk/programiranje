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

namespace Andjela_EvidencijaRadnikaA3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-2UBJC86\SQLEXPRESS;Initial Catalog=EIT_A03_EvidencijaRadnika;Integrated Security=True"); /* MM 2 sp*/

        SqlCommand kom = new SqlCommand();

        SqlDataReader dr;

        int id = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            PunimListViu();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PunimListViu()
        {
            listView1.Items.Clear();

            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniListView", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                ListViewItem red = new ListViewItem(dr[0].ToString());
                for (int i = 1; i < 6; i++) /* i IDE DO KOLIKO POLJA VRACA PROCEDURA*/
                    red.SubItems.Add(dr[i].ToString());
                listView1.Items.Add(red);
            }
            Kon.Close();

        }

        private void SaListViuNaKontrole()
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                id = Convert.ToInt32(item.SubItems[0].Text);

                textBox1.Text = id.ToString();
                textBox2.Text = item.SubItems[1].Text;
                maskedTextBox1.Text = item.SubItems[2].Text;
                textBox3.Text = item.SubItems[3].Text;
                checkBox1.Checked = Convert.ToBoolean(item.SubItems[4].Text);
                richTextBox1.Text = item.SubItems[5].Text;

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaListViuNaKontrole();
        }

        private void BrisiProjekat()
        {
            Kon.Open();
            SqlCommand cmd = new SqlCommand("BrisiProjekat", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SifraProjekta", SqlDbType.Int).Value = Convert.ToInt32(textBox1.Text.Trim());

            cmd.ExecuteNonQuery();

            Kon.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string poruka = "Zelite da obrisete projekat?";
            string naslov = "Brisanje projekta";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(poruka, naslov, buttons);

            if (result == DialogResult.Yes)
            {
                BrisiProjekat();
                PunimListViu();

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
