using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMS
{
    public partial class Form7 : Form
    {
        OleDbConnection con;
        OleDbCommand cmd;
        string un;
        DateTime now;
        bool b = false;
        int sum = 0;
        bool c = false;
        int sval;
        bool pcheck = false;
        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void accUser(string s)
        {
            un = s;
        }
        private void Form7_Load_1(object sender, EventArgs e)
        {
            var conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Record.accdb;
                           Persist Security Info=False;";
            con = new OleDbConnection(conStr);
            try
            {
                con.Open();
                try
                {
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    DataTable table = new DataTable();
                    try
                    {
                        cmd = new OleDbCommand($@"Select p.Title, u.Quantity 
                                                  From Product as p, Stock as s, [Update] as u
                                                  where s.StockID = u.StockID
                                                  and p.ProductID = u.ProductID
                                                  and u.Quantity > 0
                                                  and s.State = 1;", con);
                        adp.SelectCommand = cmd;
                        adp.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    adp.Dispose();
                    table.Dispose();
                    cmd = new OleDbCommand($@"Select StockID
                                      From Stock
                                      where State = 1;", con);
                    OleDbDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        sval = (int)rdr["StockID"];
                    }
                        cmd = new OleDbCommand($@"Select count(*) 
                                              From Product as p, Stock as s, [Update] as u
                                              where s.StockID = u.StockID
                                              and p.ProductID = u.ProductID
                                              and u.Quantity > 0
                                              and s.State = 1;", con);
                    int count = (int)cmd.ExecuteScalar();
                    cmd = new OleDbCommand($@"Select p.Title
                                              From Product as p, Stock as s, [Update] as u
                                              where s.StockID = u.StockID
                                              and p.ProductID = u.ProductID
                                              and u.Quantity > 0
                                              and s.State = 1;", con);
                    string[] arr = new string[count];
                    rdr = cmd.ExecuteReader();
                    for (int i = 0; i < count; i++)
                        if (rdr.Read())
                            arr[i] = rdr["Title"].ToString();
                    comboBox1.DataSource = arr.ToArray();
                    cmd.Dispose();
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form3();
            v.Show();
            v.accUser(un);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Quantity is Missing", "ERROR");
            }
            else
            {
                cmd = new OleDbCommand($@"Select count(*)
                                      From Product as p, Stock as s, [Update] as u
                                      where s.StockID = u.StockID
                                      and p.ProductID = u.ProductID
                                      and u.Quantity >= {textBox1.Text}
                                      and s.State = 1
                                      and p.Title = '{comboBox1.Text}';", con);
                int count = (int)cmd.ExecuteScalar();
                if (count == 1)
                {
                    if (!b)
                    {
                        now = DateTime.Now;
                        b = true;
                    }
                    try
                    {
                        cmd = new OleDbCommand($@"Update Product
                                              Set Quantity = Quantity - {textBox1.Text}
                                              where [Title] = '{comboBox1.Text}';", con);
                        cmd.ExecuteNonQuery();
                        cmd = new OleDbCommand($@"Update Stock
                                              Set Quantity = Quantity - {textBox1.Text}
                                              where State = 1;", con);
                        cmd.ExecuteNonQuery();
                        cmd = new OleDbCommand($@"Update [Update]
                                              Set Quantity = Quantity - {textBox1.Text}
                                              where ID in(Select u.ID
                                                          From Product as p, Stock as s, [Update] as u
                                                          where s.StockID = u.StockID
                                                          and p.ProductID = u.ProductID
                                                          and P.Title = '{comboBox1.Text}'
                                                          and s.State = 1);", con);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    try
                    {
                        cmd = new OleDbCommand($@"Select *
                                              From Product
                                              where Title = '{comboBox1.Text}'", con);
                        OleDbDataReader rdr = cmd.ExecuteReader();
                        int id;
                        if (rdr.Read())
                        {
                            id = (int)rdr["ProductID"];
                            sum = sum + (int)rdr["Price"] * Convert.ToInt32(textBox1.Text);
                            label5.Text = sum.ToString();
                            try
                            {
                                cmd = new OleDbCommand($@"Insert into Contain (ProductID,Quantity)
                                                   Values ({id},{textBox1.Text});", con);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception x)
                            {
                                MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    try
                    {
                        OleDbDataAdapter adp = new OleDbDataAdapter();
                        DataTable table = new DataTable();
                        try
                        {
                            cmd = new OleDbCommand($@"Select p.Title, u.Quantity 
                                                  From Product as p, Stock as s, [Update] as u
                                                  where s.StockID = u.StockID
                                                  and p.ProductID = u.ProductID
                                                  and u.Quantity > 0
                                                  and s.State = 1;", con);
                            adp.SelectCommand = cmd;
                            adp.Fill(table);
                            dataGridView1.DataSource = table;
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        adp.Dispose();
                        table.Dispose();
                        cmd = new OleDbCommand($@"Select count(*) 
                                              From Product as p, Stock as s, [Update] as u
                                              where s.StockID = u.StockID
                                              and p.ProductID = u.ProductID
                                              and u.Quantity > 0
                                              and s.State = 1;", con);
                        count = (int)cmd.ExecuteScalar();
                        cmd = new OleDbCommand($@"Select p.Title
                                              From Product as p, Stock as s, [Update] as u
                                              where s.StockID = u.StockID
                                              and p.ProductID = u.ProductID
                                              and u.Quantity > 0
                                              and s.State = 1;", con);
                        string[] arr = new string[count];
                        OleDbDataReader rdr = cmd.ExecuteReader();
                        for (int i = 0; i < count; i++)
                            if (rdr.Read())
                                arr[i] = rdr["Title"].ToString();
                        comboBox1.DataSource = arr.ToArray();
                        cmd = new OleDbCommand($@"Select StockID
                                              From Stock
                                              where State = 1;", con);
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            int si = (int)rdr["StockID"];
                            if (!c)
                            {
                                cmd = new OleDbCommand($@"Insert into Change (StockID, Quantity)
                                                  Values ({si},{textBox1.Text});", con);
                                cmd.ExecuteNonQuery();
                                c = true;
                            }
                            else
                            {
                                cmd = new OleDbCommand($@"Update Change
                                                      Set Quantity = Quantity + {textBox1.Text}
                                                      where StockID = {si};", con);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        cmd.Dispose();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show("Quantity is not enough!", "ERROR");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime ord = now;
            now = DateTime.Now;
            try
            {
                cmd = new OleDbCommand($@"Select UserID
                                          From [User]
                                          where [Username] = '{un}';", con);
                OleDbDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    int ui = (int)rdr["UserID"];
                    var d = ord.ToShortDateString();
                    var t = ord.ToString("HH:mm");
                    try
                    {
                        cmd = new OleDbCommand($@"Insert into [Order] (OrderDate,OrderTime,UserID)
                                          Values ('{d}','{t}',{ui});", con);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cmd = new OleDbCommand($@"Select OrderNumber
                                              From [Order]
                                              where OrderTime = '{t}';", con);
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        int on = (int)rdr["OrderNumber"];
                        cmd = new OleDbCommand($@"Update Contain
                                              Set OrderNumber = {on}
                                              where OrderNumber = 0;", con);
                        cmd.ExecuteNonQuery();
                        cmd = new OleDbCommand($@"Select count(*) 
                                                  from Contain
                                                  where OrderNumber = {on}", con);
                        int count = (int)cmd.ExecuteScalar();
                        cmd = new OleDbCommand($@"Select p.Title as PTitle, c.Quantity as CQuantity, c.Quantity * p.Price as TPrice
                                                  from Contain as c, Product as p
                                                  where p.ProductID = c.ProductID
                                                  and c.OrderNumber = {on}", con);
                        rdr = cmd.ExecuteReader();
                        StreamWriter invoice = File.CreateText("D:\\Invoice.txt");
                        invoice.WriteLine($"--------------------Order#{on}--------------------");
                        invoice.WriteLine($" Product                   | Quantity | Price     ");
                        for (int i = 0; i < count; i++)
                        {
                            if (rdr.Read())
                            {
                                                               
                                invoice.WriteLine($" {rdr["PTitle"]} | {rdr["CQuantity"]} | Rs. {rdr["TPrice"]}.00");
                            }
                        }
                        invoice.WriteLine($"--------------------------------------------------");
                        invoice.WriteLine($"Grand Total = Rs. {sum}.00");
                        invoice.Close();
                        d = now.ToShortDateString();
                        t = now.ToString("HH:mm");
                        cmd = new OleDbCommand($@"Insert into [Transaction] (TransactionDate,TransactionTime,TotalCost,OrderNumber,UserID)
                                                  Values ('{d}','{t}',{sum},{on},{ui});", con);
                        cmd.ExecuteNonQuery();
                        cmd = new OleDbCommand($@"Select TransactionID
                                                  From [Transaction]
                                                  where TransactionTime = '{t}';", con);
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            int ti = (int)rdr["TransactionID"];
                            cmd = new OleDbCommand($@"Update Change
                                                      Set TransactionID = {ti}
                                                      where TransactionID = 0;", con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Transaction Complete!", "Congratulation");
                            b = false;
                        }
                        label5.Text = "0.00";
                        sum = 0;
                    }
                }
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            cmd = new OleDbCommand($@"Select count(*)
                                      From Stock;", con);
            int count = (int)cmd.ExecuteScalar();
            cmd = new OleDbCommand($@"Select StockID
                                      From Stock
                                      where State = 1;", con);
            OleDbDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                int si = (int)rdr["StockID"];
                cmd = new OleDbCommand($@"Update Stock
                                          Set State = 0
                                          where StockID = {si};", con);
                cmd.ExecuteNonQuery();
                si = (si + 1) % (count + 1);
                if (si == 0)
                {
                    si++;
                }
                if (si == sval)
                {
                    pcheck = true;
                }
                cmd = new OleDbCommand($@"Update Stock
                                          Set State = 1
                                          where StockID = {si};", con);
                cmd.ExecuteNonQuery();
                if (!pcheck)
                {
                    c = false;
                }
                try
                {
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    DataTable table = new DataTable();
                    try
                    {
                        cmd = new OleDbCommand($@"Select p.Title, u.Quantity 
                                                  From Product as p, Stock as s, [Update] as u
                                                  where s.StockID = u.StockID
                                                  and p.ProductID = u.ProductID
                                                  and u.Quantity > 0
                                                  and s.State = 1;", con);
                        adp.SelectCommand = cmd;
                        adp.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    adp.Dispose();
                    table.Dispose();
                    cmd = new OleDbCommand($@"Select count(*) 
                                              From Product as p, Stock as s, [Update] as u
                                              where s.StockID = u.StockID
                                              and p.ProductID = u.ProductID
                                              and u.Quantity > 0
                                              and s.State = 1;", con);
                    count = (int)cmd.ExecuteScalar();
                    cmd = new OleDbCommand($@"Select p.Title
                                              From Product as p, Stock as s, [Update] as u
                                              where s.StockID = u.StockID
                                              and p.ProductID = u.ProductID
                                              and u.Quantity > 0
                                              and s.State = 1;", con);
                    string[] arr = new string[count];
                    rdr = cmd.ExecuteReader();
                    for (int i = 0; i < count; i++)
                        if (rdr.Read())
                            arr[i] = rdr["Title"].ToString();
                    comboBox1.DataSource = arr.ToArray();
                    cmd.Dispose();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message, "Incorrect SQL Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }
    }
}
