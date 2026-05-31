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

namespace bib2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection Kon = new SqlConnection(@"Data Source=DESKTOP-V8UI7FU\SQLEXPRESS01;Initial Catalog=EIT_А02_Biblioteka;Integrated Security=True"); /* MM 2 sp*/

        SqlCommand kom = new SqlCommand();

        SqlDataReader dr;

        int id = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;

            listView1.FullRowSelect = true;
            listView1.MultiSelect = false;
            listView1.View = View.Details;

            PuniListViu();
        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

   
        private void PuniListViu()
        {
            listView1.Items.Clear();

            Kon.Open();

            SqlCommand cmd = new SqlCommand("PuniListView", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                ListViewItem red = new ListViewItem(dr[0].ToString());
                for (int i = 1; i < 4; i++) /* i IDE DO KOLIKO POLJA VRACA PROCEDURA*/
                    red.SubItems.Add(dr[i].ToString());
                listView1.Items.Add(red);
            }
            Kon.Close();

        }

        private void SaListViuNaKontrole()
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem item = listView1.SelectedItems[0];

            textBox1.Text = item.SubItems[0].Text;
            textBox2.Text = item.SubItems[1].Text;
            textBox3.Text = item.SubItems[2].Text;
            maskedTextBox1.Text = item.SubItems[3].Text;

            id = Convert.ToInt32(item.SubItems[0].Text);
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            SaListViuNaKontrole();
        }

        private void BrisiAutora()
        {
            Kon.Open();
            SqlCommand cmd = new SqlCommand("BrisiAutor", Kon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AutorID", SqlDbType.Int).Value = Convert.ToInt32(textBox1.Text.Trim());

            cmd.ExecuteNonQuery();

            Kon.Close();
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            string poruka = "Zelite da obrisete autora?";
            string naslov = "Brisanje autora";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(poruka, naslov, buttons);

            if (result == DialogResult.Yes)
            {
                BrisiAutora();
                PuniListViu();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {

        }

    
    }
}
