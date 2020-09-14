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
    public partial class Form13 : Form
    {
        OleDbConnection con;
        OleDbCommand cmd;
        string un;
        public Form13()
        {
            InitializeComponent();
        }
        public void accUser(string s)
        {
            un = s;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form4();
            v.Show();
            v.accUser(un);
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            var conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Record.accdb;
                           Persist Security Info=False;";
            con = new OleDbConnection(conStr);
            try
            {
                con.Open();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Database File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter adp = new OleDbDataAdapter();
            DataTable table = new DataTable();
            cmd = new OleDbCommand($@"Select [Username], FirstName, LastName, Email, PhoneNumber, LastActionBy
                                      from [User];", con);
            adp.SelectCommand = cmd;
            adp.Fill(table);
            dataGridView1.DataSource = table;
            adp.Dispose();
            table.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter adp = new OleDbDataAdapter();
            DataTable table = new DataTable();
            cmd = new OleDbCommand($@"Select *
                                      from [User]
                                      where [Username] = '{un}';", con);
            adp.SelectCommand = cmd;
            adp.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void Form13_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }
    }
}
