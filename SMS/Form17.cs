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
    public partial class Form17 : Form
    {
        OleDbConnection con;
        OleDbCommand cmd;
        string un;
        public Form17()
        {
            InitializeComponent();
        }
        public void accUser(string s)
        {
            un = s;
        }
        private void Form17_Load(object sender, EventArgs e)
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
                    OleDbDataAdapter adp = new OleDbDataAdapter();
                    DataTable table = new DataTable();
                    try
                    {
                        cmd = new OleDbCommand($@"Select p.ProductID as Product_ID, p.Title as Product_Title, u.Quantity as In_Stock_Quantity, p.Price as Product_Price, p.Category as Product_Category, s.Quantity as Stock_Quantity, p.Quantity as Total_Quantity 
                                                  From Product as p, Stock as s, [Update] as u
                                                  where s.StockID = u.StockID
                                                  and p.ProductID = u.ProductID
                                                  and u.Quantity > 0
                                                  and s.StockName = '{comboBox1.Text}';", con);
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
            OleDbDataAdapter adp = new OleDbDataAdapter();
            DataTable table = new DataTable();
            try
            {
                cmd = new OleDbCommand($@"Select p.ProductID as Product_ID, p.Title as Product_Title, u.Quantity as In_Stock_Quantity, p.Price as Product_Price, p.Category as Product_Category, s.Quantity as Stock_Quantity, p.Quantity as Total_Quantity 
                                                  From Product as p, Stock as s, [Update] as u
                                                  where s.StockID = u.StockID
                                                  and p.ProductID = u.ProductID
                                                  and u.Quantity > 0
                                                  and s.StockName = '{comboBox1.Text}';", con);
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var v = new Form6();
            v.Show();
            v.accUser(un);
        }

        private void Form17_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
            con.Dispose();
        }
    }
}
