using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Andjela_DVDKolekcijaA13
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = true;
            richTextBox1.Text =
            "DVD kolekcija – Pozorište „Zvezda“\n\n" +
            "Aplikacija je namenjena pregledu i korekciji podataka o producentima, kao i uvidu u broj filmova po producentskim kućama.\n\n" +
            "Na početnoj formi nalazi se lista producenata. Klikom na producenta prikazuju se njegovi podaci u poljima. Menjaju se samo Ime i E-mail adresa, dok se šifra ne menja. Nakon izmene, klikom na Izmeni promene se upisuju u bazu i lista se osvežava.\n\n" +
            "Opcija Analiza prikazuje tabelu i grafikon broja filmova po producentu. Opcija Izlaz zatvara aplikaciju.";

        }
    }
}
