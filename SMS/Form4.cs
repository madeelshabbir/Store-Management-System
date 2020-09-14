using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMS
{
    public partial class Form4 : Form
    {
        string un;
        public Form4()
        {
            InitializeComponent();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form3();
            v.Show();
            v.accUser(un);
        }
        public void accUser(string s)
        {
            un = s;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form10();
            v.Show();
            v.accUser(un);
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form11();
            v.Show();
            v.accUser(un);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form12();
            v.Show();
            v.accUser(un);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form13();
            v.Show();
            v.accUser(un);
        }
    }
}
