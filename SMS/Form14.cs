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
    public partial class Form14 : Form
    {
        OleDbConnection con;
        OleDbCommand cmd;
        string un;
        public Form14()
        {
            InitializeComponent();
        }
        public void accUser(string s)
        {
            un = s;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int c = 0;
            if (textBox1.Text == "")
            {
                label7.Text = "Missing";
            }
            else
            {
                c++;
                label7.Text = "";
            }
            if (textBox2.Text.Length == 0)
            {
                label8.Text = "Missing";
            }
            else
            {
                c++;
                label8.Text = "";
            }
            if (textBox3.Text.Length == 0)
            {
                label9.Text = "Missing";
            }
            else
            {
                c++;
                label9.Text = "";
            }
            if(c == 3)
            {
                cmd = new OleDbCommand($@"Select UserID
                                      from [User]
                                      where [Username] = '{un}';", con);
                OleDbDataReader rdr = cmd.ExecuteReader();
                string ui = "";
                if (rdr.Read())
                {
                    ui = rdr["UserID"].ToString();
                }
                cmd = new OleDbCommand($@"Select StockID
                                      from Stock
                                      where StockName = '{comboBox1.Text}';", con);
                rdr = cmd.ExecuteReader();
                string si = "";
                if (rdr.Read())
                {
                    si = rdr["StockID"].ToString();
                }
                cmd = new OleDbCommand($@"Select count(*)
                                      From Stock as s, [Update] as u, Product as p
                                      where s.StockID = u.StockID
                                      and p.ProductID = u.ProductID
                                      and p.Title = '{textBox1.Text}'
                                      and s.StockID = {si};", con);
                int count = (int)cmd.ExecuteScalar();
                if (count == 0)
                {
                    cmd = new OleDbCommand($@"Select count(*)
                                      From Product
                                      where Title = '{textBox1.Text}';", con);
                    count = (int)cmd.ExecuteScalar();
                    if (count == 1)
                    {
                        cmd = new OleDbCommand($@"Update Product
                                              Set Quantity = Quantity + {textBox2.Text}
                                              where Title = '{textBox1.Text}';", con);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        try
                        {
                            cmd = new OleDbCommand($@"Insert into Product (Title, Quantity, Price, [Category], UserID)
                                                Values ('{textBox1.Text}', {textBox2.Text}, {textBox3.Text}, '{comboBox2.Text}', {ui});", con);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    cmd = new OleDbCommand($@"Select ProductID
                                          from Product
                                          where Title = '{textBox1.Text}'", con);
                    rdr = cmd.ExecuteReader();
                    string pi = "";
                    if (rdr.Read())
                    {
                        pi = rdr["ProductID"].ToString();
                    }
                    cmd = new OleDbCommand($@"Insert into [Update] (StockID, ProductID, Quantity)
                                          Values ({si}, {pi}, {textBox2.Text});", con);
                    cmd.ExecuteNonQuery();
                    try
                    {
                        cmd = new OleDbCommand($@"Select count(*)
                                              From Stock", con);
                        count = (int)cmd.ExecuteScalar();
                        cmd = new OleDbCommand($@"Select StockName
                                              From Stock", con);
                        string[] arr = new string[count];
                        rdr = cmd.ExecuteReader();
                        for (int i = 0; i < count; i++)
                            if (rdr.Read())
                                arr[i] = rdr["StockName"].ToString();
                        comboBox1.DataSource = arr.ToArray();
                        string[] arr1 = { "Bakery", "Grocery", "Drink", "Others" };
                        comboBox2.DataSource = arr1.ToArray();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cmd = new OleDbCommand($@"Update Stock
                                              Set Quantity = Quantity + {textBox2.Text}
                                              where StockID = {si};", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product has added in Stock","Congratulation!");
                }
                else
                {
                    label7.Text = "Alreaady Exist";
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form6();
            v.Show();
            v.accUser(un);
        }

        private void Form14_Load(object sender, EventArgs e)
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
                                              From Stock", con);
                    int count = (int)cmd.ExecuteScalar();
                    cmd = new OleDbCommand($@"Select StockName
                                              From Stock", con);
                    string[] arr = new string[count];
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    for (int i = 0; i < count; i++)
                        if (rdr.Read())
                            arr[i] = rdr["StockName"].ToString();
                    comboBox1.DataSource = arr.ToArray();
                    string[] arr1 = {"Bakery", "Grocery", "Drink", "Others"};
                    comboBox2.DataSource = arr1.ToArray();
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

        private void Form14_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var v = new Form18();
            v.Show();
            v.accUser(un);
        }
    }
}
