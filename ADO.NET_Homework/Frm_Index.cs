using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ADO.NET_Homework
{
    public partial class Frm_Index : Form
    {
        public Frm_Index()
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = 3;
            this.categoriesTableAdapter1.Fill(this.nwDataSet1.Categories);
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.customersTableAdapter1.Fill(this.nwDataSet1.Customers);
        }

        private void btnH1Con_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True"))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand("select * from Customers", conn);
                    SqlDataReader datareader = comm.ExecuteReader();
                    while (datareader.Read())
                    {
                        this.listBox1.Items.Add(datareader[0].ToString() + "\t" + datareader[1].ToString() + "\t" + datareader[2].ToString() + "\t" + datareader[3].ToString());                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnH1Discon_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source =.; Initial Catalog = Northwind; Integrated Security = True");
            SqlDataAdapter adapter = new SqlDataAdapter("select * from Customers", conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            this.dataGridView3.DataSource = ds.Tables[0];
        }

        private void btnH2Search_Click(object sender, EventArgs e)
        {
            int price1 = int.Parse(textBox1.Text);
            int price2 = int.Parse(textBox2.Text);
            this.productsTableAdapter1.FillBy(this.nwDataSet1.Products, price1, price2);
            //this.listBox2.DataSource = this.nwDataSet1.Products.ToString();
            this.dataGridView1.DataSource = this.nwDataSet1.Products;
        }
        private void btnH3Con_Click(object sender, EventArgs e)
        {
            SqlConnection con = null;
            try
            {
                cbbConnected.Items.Clear();
                con = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True");
                con.Open();
                SqlCommand com = new SqlCommand("select CategoryName from Categories", con);
                SqlDataReader datareader = com.ExecuteReader();
                while (datareader.Read())
                {
                    this.cbbConnected.Items.Add(datareader[0].ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }            
        }

        private void btnH3Dis_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True");
            SqlDataAdapter adp = new SqlDataAdapter("SELECT CategoryName from Categories", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, "CategoryName");
            cbbDisconnected.DataSource = ds.Tables["CategoryName"];
            cbbDisconnected.DisplayMember = "CategoryName";
            //comboBox1.ValueMember = "CategoryName";
        }
        
        private void cbbConnected_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBox3.Items.Clear();
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True"))
                {
                    string s = cbbConnected.Text;
                    conn.Open();
                    string sqlcomm =
                        $"select * from Products p inner join Categories c on p.CategoryId = c.CategoryId where CategoryName ='{s}'";
                    SqlCommand com = new SqlCommand(sqlcomm, conn);
                    SqlDataReader datareader = com.ExecuteReader();
                    while (datareader.Read())
                    {
                        this.listBox3.Items.Add(datareader[0].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbbDisconnected_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = cbbDisconnected.Text;
            this.productsTableAdapter1.FillByCategory(this.nwDataSet1.Products, s);
            this.dataGridView2.DataSource = this.nwDataSet1.Products;
        }

        private void btnToGridView_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source =.; Initial Catalog = Northwind; Integrated Security = True");
            SqlDataAdapter adapter1 = new SqlDataAdapter("select * from Customers", conn);
            SqlDataAdapter adapter2 = new SqlDataAdapter("select * from Products", conn);
            SqlDataAdapter adapter3 = new SqlDataAdapter("select * from Categories", conn);
            DataSet dsCustomers = new DataSet();
            adapter1.Fill(dsCustomers);
            DataSet dsProducts = new DataSet();
            adapter2.Fill(dsProducts);
            DataSet dsCategories = new DataSet();
            adapter3.Fill(dsCategories);

            this.dataGridCustomers.DataSource = dsCustomers.Tables[0];
            this.dataGridProducts.DataSource = dsProducts.Tables[0];
            this.dataGridCategories.DataSource = dsCategories.Tables[0];
        }

        private void btnToListBox_Click(object sender, EventArgs e)
        {
            int[] arrayColumn = null;//用來存每個欄位的最長字元長度
            listBox4.Items.Add(this.nwDataSet1.Tables.Count);//輸出 table 數
            string blockLine = new String('=', 250);
            string categoryLine = new String('=', 10);
            string oneLine = new String('-', 250);
            this.listBox4.Items.Add(blockLine);
            for (int i = 0; i <= nwDataSet1.Tables.Count - 1; i++)
            {
                DataTable dt = nwDataSet1.Tables[i];
                this.listBox4.Items.Add("Table ["+ dt.TableName + "]");//輸出 table 名稱
                //計算欄位最大長度
                arrayColumn = new int[dt.Columns.Count];//依 table 欄位數宣告 array 長度
                for (int column = 0; column <= dt.Columns.Count - 1; column++)//計算欄位名稱最長字數
                {
                    if (dt.Columns[column].ColumnName.Length > arrayColumn[column])
                        arrayColumn[column] = dt.Columns[column].ColumnName.Length;
                }
                for (int row = 0; row <= dt.Rows.Count - 1; row++)//計算欄位資料最長字數
                {
                    for (int j = 0; j <= dt.Columns.Count - 1; j++)
                    {
                        if (dt.Rows[row][j].ToString().Length > arrayColumn[j])
                            arrayColumn[j] = dt.Rows[row][j].ToString().Length;
                    }                    
                }                
                //開印
                string s = "";
                string sLen;
                for (int column = 0; column <= dt.Columns.Count - 1; column++)//整理欄位名稱
                {
                    sLen = "{0,-" + (arrayColumn[column]+1)+"}" ;//以最長長度+1格式化輸出
                    s += String.Format(sLen, dt.Columns[column].ColumnName);                    
                    //-arrayColumn[column
                }
                this.listBox4.Items.Add(s);//輸出欄位名稱
                this.listBox4.Items.Add(oneLine);
                for (int row = 0; row <= dt.Rows.Count - 1; row++)//印欄位資料
                {                    
                    string z = "";
                    for (int j = 0; j <= dt.Columns.Count - 1; j++)//整理欄位資料
                    {
                        sLen = "{0,-" + (arrayColumn[j] + 1) + "}";//以最長長度+1格式化字串                      
                        z += string.Format(sLen, dt.Rows[row][j]);
                        //z += string.Format("{0,-10}", dt.Rows[row][j]);
                    }
                    //listBox2.Items.Add(dt.Rows[row] + "  ");
                    this.listBox4.Items.Add(z);//輸出欄位資料
                }
                this.listBox4.Items.Add(blockLine);
            }
        }
    }
}
