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
    public partial class Form11 : Form
    {
        OleDbConnection con;
        OleDbCommand cmd;
        String un;
        bool type = false;
        int id = 1;
        bool check = false;
        string s = "";
        public Form11()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form11_Load(object sender, EventArgs e)
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
                string[] arr1 = {"Username", "Password", "First Name", "Last Name", "Pin Code", "Phone Number", "Email"};
                comboBox2.DataSource = arr1.ToArray();
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
        public void accUser(string s)
        {
            un = s;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            bool b = false;
            if (comboBox2.Text == "Username")
            {
                type = false;
                id = 1;
                s = "Username";
            }
            else if (comboBox2.Text == "Password")
            {
                type = false;
                id = 2;
                s = "Password";
            }
            else if (comboBox2.Text == "First Name")
            {
                type = false;
                id = 3;
                s = "First Name";
            }
            else if (comboBox2.Text == "Last Name")
            {
                type = false;
                id = 4;
                s = "Last Name";
            }
            else if (comboBox2.Text == "Pin Code")
            {
                id = 5;
                type = true;
                s = "Pin Code";
            }
            else if (comboBox2.Text == "Phone Number")
            {
                id = 6;
                type = true;
                s = "Phone No";
            }
            else if (comboBox2.Text == "Email")
            {
                type = false;
                id = 7;
                s = "Email";
            }
            else
                b = true;
            if (!b)
            {
                label5.Text = $"New {s}:";
                b = false;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (type)
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count;
            bool accept = false;
            if (id == 1)
            {
                cmd = new OleDbCommand($@"Select count(*)
                                              From [User]
                                              where [Username] = '{textBox2.Text}';", con);
                count = (int)cmd.ExecuteScalar();
                if (count == 1)
                {
                    label7.Text = "Already Exist";
                }
                else if (textBox2.Text == "")
                {
                    label7.Text = "Missing";
                }
                else
                {
                    if (textBox2.Text.Length > 18)
                        label7.Text = "Max Limit is 18 Digits";
                    else
                    {
                        accept = true;
                        label7.Text = "";
                    }
                }
            }
            else if (id == 2)
            {
                if (textBox2.Text.Length < 8)
                {
                    label7.Text = "Weak";
                    accept = true;
                }
                else if (textBox2.Text == "")
                {
                    label7.Text = "Missing";
                }
                else if (textBox2.Text.Length > 18)
                    label7.Text = "Max Limit is 18 Digits";
                else
                {
                    accept = true;
                    label7.Text = "";
                }
            }
            else if (id == 3 || id == 4)
            {
                if (textBox2.Text == "")
                {
                    label7.Text = "Missing";
                }
                else
                {
                    accept = true;
                    label7.Text = "";
                }
            }
            else if (id == 5)
            {
                if (textBox2.Text.Length == 4)
                {
                    accept = true;
                    label7.Text = "";
                }
                else if (textBox2.Text == "")
                {
                    label7.Text = "Missing";
                }
                else
                {
                    label7.Text = "Enter 4 Digits";
                }
            }
            else if (id == 6)
            {
                if (textBox2.Text.Length == 11)
                {
                    accept = true;
                    label7.Text = "";
                }
                else if (textBox2.Text == "")
                {
                    label7.Text = "Missing";
                }
                else
                {
                    label7.Text = "Enter 11 Digits";
                }
            }
            else if (id == 7)
            {
                if (textBox2.Text.IndexOf('@') == -1)
                    label7.Text = "Invalid";
                else if (textBox2.Text.IndexOf('@') != -1)
                {
                    if (textBox2.Text.IndexOf('@', textBox2.Text.IndexOf('@') + 1) != -1)
                        label7.Text = "Invalid";
                    else if (textBox2.Text.IndexOf('.', textBox2.Text.IndexOf('@') + 1) != -1)
                    {
                        accept = true;
                        label7.Text = "";
                    }
                    else if (textBox2.Text == "")
                    {
                        label7.Text = "Missing";
                    }
                    else
                    {
                        label2.Text = "Invalid";
                    }
                }
            }
            cmd = new OleDbCommand($@"Select count(*)
                                      From [User]
                                      where [Username] = '{comboBox1.Text}'
                                      and [Password] = '{textBox1.Text}';", con);
            count = (int)cmd.ExecuteScalar();
            if (count == 1 && accept)
            {
                accept = true;
                label6.Text = "";
            }
            else if(count == 1)
            {
                label6.Text = "";
            }
            else
            {
                accept = false;
                label6.Text = "Incorrect Password";
            }
            if (accept)
            {
                if (id == 1)
                    cmd = new OleDbCommand($@"Update [User]
                                          Set [Username] = '{textBox2.Text}'
                                          where [Username] = '{comboBox1.Text}';", con);
                else if (id == 2)
                    cmd = new OleDbCommand($@"Update [User]
                                          Set [Password] = '{textBox2.Text}'
                                          where [Username] = '{comboBox1.Text}';", con);
                else if (id == 3)
                    cmd = new OleDbCommand($@"Update [User]
                                          Set FirstName = '{textBox2.Text}'
                                          where [Username] = '{comboBox1.Text}';", con);
                else if (id == 4)
                    cmd = new OleDbCommand($@"Update [User]
                                          Set LastName = '{textBox2.Text}'
                                          where [Username] = '{comboBox1.Text}';", con);
                else if(id == 5)
                    cmd = new OleDbCommand($@"Update [User]
                                          Set PinCode = '{textBox2.Text}'
                                          where [Username] = '{comboBox1.Text}';", con);
                else if(id == 6)
                    cmd = new OleDbCommand($@"Update [User]
                                          Set PhoneNumber = '{textBox2.Text}'
                                          where [Username] = '{comboBox1.Text}';", con);
                else if(id == 7)
                    cmd = new OleDbCommand($@"Update [User]
                                          Set Email = '{textBox2.Text}'
                                          where [Username] = '{comboBox1.Text}';", con);
                cmd.ExecuteNonQuery();
                cmd = new OleDbCommand($@"Select UserID
                                          From [User]
                                          where [Username] = '{un}';", con);
                OleDbDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    int ui = (int)rdr["UserID"];
                    cmd = new OleDbCommand($@"Update [User]
                                              Set LastActionBy = {ui}
                                              where [Username] = '{comboBox1.Text}';", con);
                    cmd.ExecuteNonQuery();
                }
                try
                {
                    cmd = new OleDbCommand($@"Select count(*)
                                              From [User]", con);
                    count = (int)cmd.ExecuteScalar();
                    cmd = new OleDbCommand($@"Select [Username]
                                              From [User]", con);
                    string[] arr = new string[count];
                    rdr = cmd.ExecuteReader();
                    for (int i = 0; i < count; i++)
                        if (rdr.Read())
                            arr[i] = rdr["Username"].ToString();
                    comboBox1.DataSource = arr.ToArray();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show(s + " has Successfully Updated", "Congratulations!");
                accept = false;
            }
        }

        private void Form11_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }
    }
}
