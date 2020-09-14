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
    public partial class Form6 : Form
    {
        string un;
        public Form6()
        {
            InitializeComponent();
        }
        public void accUser(string s)
        {
            un = s;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form14();
            v.Show();
            v.accUser(un);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form16();
            v.Show();
            v.accUser(un);
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form3();
            v.Show();
            v.accUser(un);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form15();
            v.Show();
            v.accUser(un);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form17();
            v.Show();
            v.accUser(un);
        }
    }
}
