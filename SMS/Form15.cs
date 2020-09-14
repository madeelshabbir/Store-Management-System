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
    public partial class Form15 : Form
    {
        OleDbConnection con;
        OleDbCommand cmd;
        String un;
        public Form15()
        {
            InitializeComponent();
        }
        public void accUser(string s)
        {
            un = s;
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Form15_Load(object sender, EventArgs e)
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
                    cmd = new OleDbCommand($@"Select count(*) 
                                      From Product as p, Stock as s, [Update] as u
                                      where s.StockID = u.StockID
                                      and p.ProductID = u.ProductID
                                      and s.StockName = '{comboBox1.Text}';", con);
                    count = (int)cmd.ExecuteScalar();
                    cmd = new OleDbCommand($@"Select p.Title
                                      From Product as p, Stock as s, [Update] as u
                                      where s.StockID = u.StockID
                                      and p.ProductID = u.ProductID
                                      and s.StockName = '{comboBox1.Text}';", con);
                    string[] arr1 = new string[count];
                    rdr = cmd.ExecuteReader();
                    for (int i = 0; i < count; i++)
                        if (rdr.Read())
                            arr1[i] = rdr["Title"].ToString();
                    comboBox2.DataSource = arr1.ToArray();
                    string[] arr2 = { "Quantity", "Price" };
                    comboBox3.DataSource = arr2.ToArray();
                    label5.Text = "Enter New Quantity:";
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmd = new OleDbCommand($@"Select count(*) 
                                      From Product as p, Stock as s, [Update] as u
                                      where s.StockID = u.StockID
                                      and p.ProductID = u.ProductID
                                      and s.StockName = '{comboBox1.Text}';", con);
            int count = (int)cmd.ExecuteScalar();
            cmd = new OleDbCommand($@"Select p.Title
                                      From Product as p, Stock as s, [Update] as u
                                      where s.StockID = u.StockID
                                      and p.ProductID = u.ProductID
                                      and s.StockName = '{comboBox1.Text}';", con);
            string[] arr = new string[count];
            OleDbDataReader rdr = cmd.ExecuteReader();
            for (int i = 0; i < count; i++)
                if (rdr.Read())
                    arr[i] = rdr["Title"].ToString();
            comboBox2.DataSource = arr.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form6();
            v.Show();
            v.accUser(un);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.Text == "Quantity")
            {
                label5.Text = "Enter New Quantity:";
            }
            else
            {
                label5.Text = "Enter New Price";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                label6.Text = "Missing";
            }
            else
            {
                label6.Text = "";
                cmd = new OleDbCommand($@"Select UserID
                                          from [User]
                                          where [Username] = '{un}';", con);
                OleDbDataReader rdr = cmd.ExecuteReader();
                int ui = 0;
                if (rdr.Read())
                {
                    ui = (int)rdr["UserID"];
                }
                if (comboBox3.Text == "Quantity")
                {
                    cmd = new OleDbCommand($@"Select u.Quantity as UQuantity, u.ID as UPID
                                      From Product as p, Stock as s, [Update] as u
                                      where s.StockID = u.StockID
                                      and p.ProductID = u.ProductID
                                      and s.StockName = '{comboBox1.Text}';", con);
                    rdr = cmd.ExecuteReader();
                    int pq = 0;
                    string u = "";
                    if (rdr.Read())
                    {
                        pq = (int)rdr["UQuantity"];
                        u = rdr["UPID"].ToString();
                    }
                    cmd = new OleDbCommand($@"Update Product
                                          Set Quantity = Quantity + {textBox1.Text} - {pq}, UserID = {ui}
                                          where Title = '{comboBox2.Text}'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new OleDbCommand($@"Update Stock
                                          Set Quantity = Quantity + {textBox1.Text} - {pq}
                                          where StockName = '{comboBox1.Text}'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new OleDbCommand($@"Update [Update]
                                          Set Quantity = {textBox1.Text}
                                          where ID = {u};", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Quantity has Successfully Updated", "Congratulations!");
                }
                else
                {
                    cmd = new OleDbCommand($@"Update Product
                                          Set Price = {textBox1.Text}, UserID = {ui}
                                          where Title = '{comboBox2.Text}'", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Price has Successfully Updated", "Congratulations!");
                }
                try
                {
                    cmd = new OleDbCommand($@"Select count(*)
                                              From Stock", con);
                    int count = (int)cmd.ExecuteScalar();
                    cmd = new OleDbCommand($@"Select StockName
                                              From Stock", con);
                    string[] arr = new string[count];
                    rdr = cmd.ExecuteReader();
                    for (int i = 0; i < count; i++)
                        if (rdr.Read())
                            arr[i] = rdr["StockName"].ToString();
                    comboBox1.DataSource = arr.ToArray();
                    cmd = new OleDbCommand($@"Select count(*) 
                                      From Product as p, Stock as s, [Update] as u
                                      where s.StockID = u.StockID
                                      and p.ProductID = u.ProductID
                                      and s.StockName = '{comboBox1.Text}';", con);
                    count = (int)cmd.ExecuteScalar();
                    cmd = new OleDbCommand($@"Select p.Title
                                      From Product as p, Stock as s, [Update] as u
                                      where s.StockID = u.StockID
                                      and p.ProductID = u.ProductID
                                      and s.StockName = '{comboBox1.Text}';", con);
                    string[] arr1 = new string[count];
                    rdr = cmd.ExecuteReader();
                    for (int i = 0; i < count; i++)
                        if (rdr.Read())
                            arr1[i] = rdr["Title"].ToString();
                    comboBox2.DataSource = arr1.ToArray();
                    string[] arr2 = { "Quantity", "Price" };
                    comboBox3.DataSource = arr2.ToArray();
                    label5.Text = "Enter New Quantity:";
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form15_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }
    }
}
