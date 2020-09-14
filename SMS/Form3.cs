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
    public partial class Form3 : Form
    {
        string un;
        public Form3()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form4();
            v.Show();
            v.accUser(un);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form5();
            v.Show();
            v.accUser(un);
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form6();
            v.Show();
            v.accUser(un);
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form7();
            v.Show();
            v.accUser(un);
        }
        public void accUser(string s)
        {
            un = s;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form8();
            v.Show();
            v.accUser(un);
        }
    }
}
