using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMS
{
    public partial class Form9 : Form
    {
        OleDbConnection con;
        string un;
        public Form9()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        public void accUser(string s)
        {
            un = s;
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            var conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Record.accdb;
                           Persist Security Info=False;";
            con = new OleDbConnection(conStr);
            try
            {
                con.Open();
                DateTime now = DateTime.Now;
                var date = now.ToShortDateString();
                OleDbCommand cmd = new OleDbCommand($@"Select sum(t.TotalCost)
                                                   From [Transaction] as t, [User] as u
                                                   where u.UserID = t.UserID
                                                   and u.Username = '{un}'
                                                   and t.TransactionDate like '{date.Substring(0, date.IndexOf('/'))}/%/{date.Substring(date.IndexOf('/', 3) + 1, 4)}';", con);
                var sum = cmd.ExecuteScalar();
                label3.Text = sum.ToString();
                if (label3.Text == "")
                    label3.Text = "0.00";
                cmd = new OleDbCommand($@"Select sum(t.TotalCost)
                                          From [Transaction] as t, [User] as u
                                          where u.UserID = t.UserID
                                          and u.Username = '{un}'
                                          and t.TransactionDate like '%{date.Substring(date.IndexOf('/', 3) + 1, 4)}';", con);
                sum = cmd.ExecuteScalar();
                label4.Text = sum.ToString();
                if (label4.Text == "")
                    label4.Text = "0.00";
                cmd.Dispose();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Database File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form3();
            v.Show();
            v.accUser(un);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form3();
            v.Show();
            v.accUser(un);
        }

        private void Form9_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }
    }
}
