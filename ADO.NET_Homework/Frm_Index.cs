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
using ADO.NET_Homework.Properties;

namespace ADO.NET_Homework
{
    public partial class Frm_Index : Form
    {
        public Frm_Index()
        {
            InitializeComponent();

            this.tabControl1.SelectedIndex = 5; //預設tabControl 之 Index
            this.categoriesTableAdapter1.Fill(this.nwDataSet1.Categories);
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.customersTableAdapter1.Fill(this.nwDataSet1.Customers);
            this.listView6.View = View.Details;
            
            LoadCountryToComboBox();
            CreateListViewColumnHeader();
            Closing += new CancelEventHandler(Frm_Index_Closing);
        }
        void Frm_Index_Closing(object sender, CancelEventArgs e)
        {
            //MessageBox.Show("Closing event\n");
            DialogResult dr = MessageBox.Show("確定要關閉程式嗎?", "Closing event!", MessageBoxButtons.YesNo);
            //way3
            e.Cancel = (dr == DialogResult.No);
            //way2
            //e.Cancel = (dr == DialogResult.No) ? true : false;
            //way1
            //if (dr == DialogResult.No)
            //    e.Cancel = true;//取消離開
            //else
            //    e.Cancel = false;
        }
        //Homework1
        private void btnH1Con_Click(object sender, EventArgs e)
        {
            try
            {
                //using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True"))
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
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
            try
            {
                //SqlConnection conn = new SqlConnection("Data Source =.; Initial Catalog = Northwind; Integrated Security = True");
                SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString);
                SqlDataAdapter adapter = new SqlDataAdapter("select * from Customers", conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                this.dataGridView3.DataSource = ds.Tables[0];
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Homework2
        private void btnH2Search_Click(object sender, EventArgs e)
        {
            int price1, price2;
            if (Int32.TryParse(textBox1.Text, out int number1) && Int32.TryParse(textBox2.Text, out int number2))
            {
                price1 = number1;
                price2 = number2;
            }
            else
            {
                MessageBox.Show("請輸入價錢區間");
                return;
            }            
            this.productsTableAdapter1.FillByDate(this.nwDataSet1.Products, price1, price2);
            this.listBox2.DataSource = this.nwDataSet1.Products;
            this.listBox2.DisplayMember = "ProductID";
            this.dataGridView1.DataSource = this.nwDataSet1.Products;
        }
        //Homework3
        private void btnH3Con_Click(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            try
            {
                cbbConnected.Items.Clear();
                //con = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True");
                conn = new SqlConnection(Settings.Default.NorthwindConnectionString);
                conn.Open();
                SqlCommand com = new SqlCommand("select CategoryName from Categories", conn);
                SqlDataReader datareader = com.ExecuteReader();
                while (datareader.Read())
                {
                    this.cbbConnected.Items.Add(datareader[0].ToString());
                }
                this.cbbConnected.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }            
        }        
        private void btnH3Dis_Click(object sender, EventArgs e)
        {
            try
            {
                //SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True");
                SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString);
                SqlDataAdapter adp = new SqlDataAdapter("SELECT CategoryName from Categories", conn);
                DataSet ds = new DataSet();
                adp.Fill(ds, "CategoryName");
                cbbDisconnected.DataSource = ds.Tables["CategoryName"];
                cbbDisconnected.DisplayMember = "CategoryName";
                //comboBox1.ValueMember = "CategoryName";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        
        private void cbbConnected_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBox3.Items.Clear();
            try
            {                
                //using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True"))
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
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
        //Homework4
        private void btnToGridView_Click(object sender, EventArgs e)
        {
            string connStr = Settings.Default.NorthwindConnectionString;
            //SqlConnection conn = new SqlConnection("Data Source =.; Initial Catalog = Northwind; Integrated Security = True");
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connStr;
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
            listBox4.Items.Add("Tables number = " + this.nwDataSet1.Tables.Count);//輸出 table 數
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
                    //way2
                    arrayColumn[column] = (dt.Columns[column].ColumnName.Length > arrayColumn[column]) ? dt.Columns[column].ColumnName.Length : arrayColumn[column];
                    //way1
                    //if (dt.Columns[column].ColumnName.Length > arrayColumn[column])
                    //    arrayColumn[column] = dt.Columns[column].ColumnName.Length;
                }
                for (int row = 0; row <= dt.Rows.Count - 1; row++)//計算欄位資料最長字數
                {
                    for (int j = 0; j <= dt.Columns.Count - 1; j++)
                    {
                        //way2
                        arrayColumn[j] = (dt.Rows[row][j].ToString().Length > arrayColumn[j]) ? dt.Rows[row][j].ToString().Length : arrayColumn[j];
                        //way1
                        //if (dt.Rows[row][j].ToString().Length > arrayColumn[j])
                        //    arrayColumn[j] = dt.Rows[row][j].ToString().Length;
                    }                    
                }                
                //開印
                string s = "";
                string sLen;
                for (int column = 0; column <= dt.Columns.Count - 1; column++)//整理欄位名稱
                {
                    sLen = "{0,-" + (arrayColumn[column]+1)+"}" ;//以最長長度+1格式化輸出                    
                    s += String.Format(sLen, dt.Columns[column].ColumnName);
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
        BindingSource bs = null;
        private void Frm_Index_Load(object sender, EventArgs e)
        {
            bs = new BindingSource();
            // TODO: 這行程式碼會將資料載入 'aWDataSet1.ProductPhoto' 資料表。您可以視需要進行移動或移除。
            this.productPhotoTableAdapter.Fill(this.aWDataSet1.ProductPhoto);
            this.bs.DataSource = this.aWDataSet1.ProductPhoto;
            this.productPhotoDataGridView.DataSource = this.bs;
            this.bindingNavigator1.BindingSource = this.bs;

            SqlConnection conn = null;
            string connStr = Settings.Default.AdventureWorksConnectionString;
            try
            {
                conn = new SqlConnection();
                comboBox1.Items.Clear();
                //conn = new SqlConnection("Data Source=.;Initial Catalog=AdventureWorks;Integrated Security=True");
                conn.ConnectionString = connStr;
                conn.Open();
                SqlCommand com = new SqlCommand("select distinct year(ModifiedDate) from [Production].[ProductPhoto]", conn);
                SqlDataReader datareader = com.ExecuteReader();
                while (datareader.Read())
                {
                    this.comboBox1.Items.Add(datareader[0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            //this.comboBox1.Items.Add(123);
        }
        //Homework5
        private void button1_Click(object sender, EventArgs e)
        {            
            //string dtp1 = dateTimePicker1.Text;
            //string dtp2 = dateTimePicker2.Text;
            //string[] arrayDtp1 = dtp1.Split(new char[3] { '年', '月','日'});
            //string[] arrayDtp2 = dtp2.Split(new char[3] { '年', '月', '日' });
            //arrayDtp1[1] = arrayDtp1[1].PadLeft(2, '0');
            //arrayDtp1[2] = arrayDtp1[2].PadLeft(2, '0');
            //arrayDtp2[1] = arrayDtp2[1].PadLeft(2, '0');
            //arrayDtp2[2] = arrayDtp2[2].PadLeft(2, '0');
            //dtp1 = arrayDtp1[0] + arrayDtp1[1] + arrayDtp1[2];
            //dtp2 = arrayDtp2[0] + arrayDtp2[1] + arrayDtp2[2];

            this.productPhotoTableAdapter.FillBy(this.aWDataSet1.ProductPhoto, dateTimePicker1.Value, dateTimePicker2.Value);
            this.productPhotoDataGridView.DataSource = this.aWDataSet1.ProductPhoto;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //this.productPhotoTableAdapter.FillBy1(this.aWDataSet1.ProductPhoto, Convert.ToInt32(comboBox1.Text));
            this.productPhotoTableAdapter.FillBy1(this.aWDataSet1.ProductPhoto, Convert.ToDecimal(comboBox1.Text));
            this.productPhotoDataGridView.DataSource = this.aWDataSet1.ProductPhoto;
            //this.productPhotoTableAdapter.FillByCount(this.aWDataSet1.ProductPhoto, Convert.ToDecimal(comboBox1.Text));
            //this.productPhotoDataGridView.DataSource = this.aWDataSet1.ProductPhoto;
        }
        //Homework6
        private void LoadCountryToComboBox()
        {
            try
            {
                string connString = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = null;
                    command = new SqlCommand("Select distinct Country from Customers", conn);
                    SqlDataReader dataReader = command.ExecuteReader();
                    this.cbb6.Items.Clear();
                    this.cbb6.Items.Add("All Country");
                    while (dataReader.Read())
                    {
                        this.cbb6.Items.Add(dataReader["Country"]);
                    }
                    this.cbb6.SelectedIndex = 0;
                } //Auton conn.close(); conn.Dispose()
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void CreateListViewColumnHeader()
        {
            listView6.ContextMenuStrip = ctms;
            listView6.LargeImageList = ImageList1;
            listView6.SmallImageList = ImageList2;
            string connString = Settings.Default.NorthwindConnectionString;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connString;
                conn.Open();
                SqlCommand comm = new SqlCommand("select * from Customers", conn);
                SqlDataReader dr = comm.ExecuteReader();
                DataTable dt = dr.GetSchemaTable();
                this.dataGridView1.DataSource = dt;

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    this.listView6.Columns.Add(dt.Rows[i][0].ToString());
                }
                this.listView6.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void cbb6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string connString = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    listView6.Groups.Clear();
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = null;
                    if (cbb6.Text == "All Country")
                        command = new SqlCommand($"Select * from Customers", conn);
                    else
                        command = new SqlCommand($"Select * from Customers where Country='{this.cbb6.Text }'", conn);
                    SqlDataReader dataReader = command.ExecuteReader();
                    this.listView6.Items.Clear();
                    Random r = new Random();
                    while (dataReader.Read())
                    {                        
                        listView6.Groups.Add(new ListViewGroup(this.cbb6.Text));
                        ListViewItem lv = this.listView6.Items.Add(dataReader[0].ToString());
                        lv.Group = this.listView6.Groups[0];
                        //MessageBox.Show(listView6.Groups[0].ToString());
                        lv.ImageIndex = r.Next(0, this.ImageList1.Images.Count);
                        //way1
                        lv.BackColor = (lv.Index % 2 == 0) ? Color.Orange : Color.LightGray;
                        //way2
                        //if (lv.Index % 2 == 0)
                        //{
                        //    lv.BackColor = Color.Orange;
                        //}
                        //else
                        //{
                        //    lv.BackColor = Color.LightGray;
                        //}

                        for (int i = 1; i <= dataReader.FieldCount - 1; i++)
                        {
                            //way1
                            lv.SubItems.Add((dataReader.IsDBNull(i)) ? "Null" : dataReader[i].ToString());
                            //way2
                            //string value = (dataReader.IsDBNull(i)) ? "Null" : dataReader[i].ToString();
                            //lv.SubItems.Add(value);
                            //way3
                            //if (dataReader.IsDBNull(i))
                            //{
                            //    lv.SubItems.Add("Null");
                            //}
                            //else
                            //{
                            //    lv.SubItems.Add(dataReader[i].ToString());
                            //}
                        }
                    }
                    this.listView6.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                } //Auton conn.close(); conn.Dispose()
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void largeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView6.View = View.LargeIcon;
        }

        private void smallIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView6.View = View.SmallIcon;
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView6.View = View.Details;
        }
    }
}
