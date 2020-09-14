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
    public partial class Form18 : Form
    {
        OleDbConnection con;
        OleDbCommand cmd;
        string un;
        public Form18()
        {
            InitializeComponent();
        }
        public void accUser(string s)
        {
            un = s;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form14();
            v.Show();
            v.accUser(un);
        }

        private void Form18_Load(object sender, EventArgs e)
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
            if(textBox1.Text == "")
            {
                MessageBox.Show("Null can't be added in Stock", "Sorry!");
            }
            else
            {
                cmd = new OleDbCommand($@"Select count(*)
                                      From Stock
                                      where StockName = '{textBox1.Text}';", con);
                int count = (int)cmd.ExecuteScalar();
                if(count == 1)
                {
                    MessageBox.Show("Stock Already Exists", "Sorry!");
                }
                else
                {
                    var now = DateTime.Now.ToShortDateString();
                    cmd = new OleDbCommand($@"Select count(*)
                                              From Stock;", con);
                    count = (int)cmd.ExecuteScalar();
                    if(count == 0)
                    {
                        cmd = new OleDbCommand($@"Insert into Stock (StockName, StockDate, State)
                                              Values ('{textBox1.Text}', '{now}', 1)", con);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new OleDbCommand($@"Insert into Stock (StockName, StockDate)
                                              Values ('{textBox1.Text}', '{now}')", con);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Stock has Successfully Generated", "Congratulations!");
                }
            }
        }

        private void Form18_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }
    }
}
