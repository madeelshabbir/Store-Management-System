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
    public partial class Form12 : Form
    {
        OleDbConnection con;
        OleDbCommand cmd;
        string un;
        public Form12()
        {
            InitializeComponent();
        }
        public void accUser(string s)
        {
            un = s;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            cmd = new OleDbCommand($@"Select count(*)
                                      From [User]
                                      where [Username] = '{comboBox1.Text}'
                                      and [Password] = '{textBox1.Text}';", con);
            int count = (int)cmd.ExecuteScalar();
            if (count == 1)
            {
                cmd = new OleDbCommand($@"Delete from [User]
                                           where [Username] = '{comboBox1.Text}';", con);
                cmd.ExecuteNonQuery();
                label4.Text = "";
                string na = comboBox1.Text;
                if (un == comboBox1.Text)
                {
                    this.Close();
                    var v = new Form1();
                    v.Show();
                }
                else
                {
                    try
                    {
                        cmd = new OleDbCommand($@"Select count(*)
                                              From [User]", con);
                        count = (int)cmd.ExecuteScalar();
                        cmd = new OleDbCommand($@"Select [Username]
                                              From [User]", con);
                        string[] arr = new string[count];
                        OleDbDataReader rdr = cmd.ExecuteReader();
                        for (int i = 0; i < count; i++)
                            if (rdr.Read())
                                arr[i] = rdr["Username"].ToString();
                        comboBox1.DataSource = arr.ToArray();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                MessageBox.Show($"User: {na} has Successfully Deleted", "Congratulations!");
            }
            else
            {
                label4.Text = "Incorrect Password";
            }
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            var conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Record.accdb;
                           Persist Security Info=False;";
            con = new OleDbConnection(conStr);
            try
            {
                con.Open();
                try
                {
                    cmd = new OleDbCommand($@"Select count(*)
                                              From [User]", con);
                    int count = (int)cmd.ExecuteScalar();
                    cmd = new OleDbCommand($@"Select [Username]
                                              From [User]", con);
                    string[] arr = new string[count];
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    for (int i = 0; i < count; i++)
                        if (rdr.Read())
                            arr[i] = rdr["Username"].ToString();
                    comboBox1.DataSource = arr.ToArray();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Database File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form4();
            v.Show();
            v.accUser(un);
        }
    }
}
