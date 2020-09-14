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
    public partial class Form10 : Form
    {
        OleDbConnection con;
        string un;
        public Form10()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form4();
            v.Show();
            v.accUser(un);
        }
        public void accUser(string s)
        {
            un = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int c = 0;
            OleDbCommand cmd = new OleDbCommand($@"Select count(*)
                                                   From [User]
                                                   where [Username] = '{textBox1.Text}';", con);
            int count = (int)cmd.ExecuteScalar();
            if (count == 1)
                label9.Text = "Already Exist";
            else if (textBox1.Text == "")
                label9.Text = "Missing";
            else
            {
                c++;
                label9.Text = "";
            }
            if (textBox2.Text == "")
                label10.Text = "Missing";
            else
            {
                c++;
                label10.Text = "";
            }
            if (textBox3.Text == "")
                label11.Text = "Missing";
            else
            {
                c++;
                label11.Text = "";
            }
            if (textBox4.Text == "")
                label12.Text = "Missing";
            else if (textBox4.Text.Length < 8)
            {
                c++;
                label12.Text = "Weak";
            }
            else
            {
                c++;
                label12.Text = "";
            }
            if (textBox5.Text.Length == 0)
                label13.Text = "Missing";
            else if (textBox5.Text.Length < 4)
                label13.Text = "Less than 4 Digits";
            else
            {
                c++;
                label13.Text = "";
            }
            if (textBox6.Text.Length == 0)
                label14.Text = "Missing";
            else if (textBox6.Text.Length < 11)
                label14.Text = "Incomplete";
            else
            {
                c++;
                label14.Text = "";
            }
            if (textBox7.Text == "")
                label15.Text = "Missing";
            else if (textBox7.Text.IndexOf('@') == -1)
                label15.Text = "Invalid";
            else if (textBox7.Text.IndexOf('@') != -1)
            {
                if (textBox7.Text.IndexOf('@', textBox7.Text.IndexOf('@') + 1) != -1)
                    label15.Text = "Invalid";
                else if(textBox7.Text.IndexOf('.', textBox7.Text.IndexOf('@') + 1) != -1)
                {
                    c++;
                    label15.Text = "";
                }
                else
                {
                    label15.Text = "Invalid";
                }
            }
            if (c == 7)
            {
                cmd = new OleDbCommand($@"Select UserID
                                          From [User]
                                          where [Username] = '{un}';", con);
                OleDbDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    int ui = (int)rdr["UserID"];
                    try
                    {
                        cmd = new OleDbCommand($@"Insert into [User] ([Username], FirstName, LastName, [Password], PinCode, PhoneNumber, Email, LastActionBy)
                                                  Values ('{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', '{textBox4.Text}', '{textBox5.Text}', '{textBox6.Text}', '{textBox7.Text}', {ui});", con);
                        cmd.ExecuteNonQuery();
                        this.Close();
                        var v = new Form4();
                        v.Show();
                        v.accUser(un);
                        MessageBox.Show("Successfully Registered", "Congratulation!");
                    }
                    catch(Exception x)
                    {
                        MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void Form10_Load(object sender, EventArgs e)
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

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void Form10_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }
    }
}
