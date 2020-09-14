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
    public partial class Form5 : Form
    {
        OleDbConnection con;
        string un;
        public Form5()
        {
            InitializeComponent();
        }
        public void accUser(string s)
        {
            un = s;
        }
        private void Form5_Load(object sender, EventArgs e)
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            OleDbCommand cmd;
            OleDbDataAdapter adp = new OleDbDataAdapter();
            DataTable table = new DataTable();
            var date = dateTimePicker1.Value.ToShortDateString();
            try
            {
                cmd = new OleDbCommand($@"Select * 
                                          From [Transaction] 
                                          where TransactionDate = '{date}';", con);
                adp.SelectCommand = cmd;
                adp.Fill(table);
                dataGridView1.DataSource = table;
                cmd.Dispose();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            adp.Dispose();
            table.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form3();
            v.Show();
            v.accUser(un);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
