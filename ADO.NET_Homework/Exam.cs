using ADO.NET_Homework.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO.NET_Homework
{
    public partial class Exam : Form
    {
        public Exam()
        {
            InitializeComponent();            
            //Settings.Default.NorthwindConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True";
            //Settings.Default.AdventureWorksConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True";

            this.tsBtn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;//修改按鈕顯示圖片變為文字
            this.tsBtn2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;//修改按鈕顯示圖片變為文字
            this.tsBtn1.Text = "TreeView";//修改按鈕文字
            this.tsBtn2.Text = "ListView";//修改按鈕文字
            this.label1.Text = "";
            //this.customersTableAdapter1.Fill(this.nwDataSet1.Customers);
            //this.bindingSource1.DataSource = this.nwDataSet1.Customers;
            //this.customersDataGridView.DataSource = this.bindingSource1;
            //this.bindingNavigator1.BindingSource = this.bindingSource1;
        }
        private void Exam_Load(object sender, EventArgs e)//倒資料
        {
            customersTableAdapter1.Fill(nwDataSet1.Customers);
            //Image bit = new Bitmap("Loading.gif");
            //this.pictureBox1.Image = bit;
        }

        private void tsBtn1_Click(object sender, EventArgs e)// TreeView 按鈕
        {
            treeView1.Nodes.Clear();// 清空 TreeView
            for (int i = 0; i <= nwDataSet1.Customers.Rows.Count - 1; i++)
            {
                string country = nwDataSet1.Customers[i].Country;
                string city = nwDataSet1.Customers[i].City;
                
                TreeNode treeNode1 = treeView1.Nodes[country];// 找第一層節點
                if (treeNode1 == null)// 如果該國家節點不存在就新增
                {
                    treeNode1 = treeView1.Nodes.Add(country, country);
                    treeNode1.Tag = 0;// Country 起始人數                    
                }

                TreeNode treeNode2 = treeView1.Nodes[country].Nodes[city];//找第二層節點                                
                if (treeNode2 == null)// 如果該城市節點不存在就新增
                {
                    treeNode2 = treeNode1.Nodes.Add(city, city);
                    treeNode2.Tag = 0;// City 起始人數
                }
                
                treeNode1.Tag = (int)treeNode1.Tag + 1;//累加 Country 人數
                treeNode2.Tag = (int)treeNode2.Tag + 1;//累加 City 人數
                treeNode1.Text = $"{country} ({treeNode1.Tag} Customers)";
                treeNode2.Text = $"{city} ({treeNode2.Tag} Customers)";
            }
        }

        private void tsBtn2_Click(object sender, EventArgs e)//ListView 按鈕
        {
            listView1.Items.Clear();//清空 Item
            listView1.Groups.Clear();//清空 Groups
            Cursor = Cursors.WaitCursor;//設定游標圖片            
            listView1.Visible = false;//隱藏 ListView 先顯示背景圖
            for (int i = 0; i <= nwDataSet1.Customers.Rows.Count - 1; i++)
            {
                string country = nwDataSet1.Customers[i].Country;
                //string city = nwDataSet1.Customers[i].City;
                ListViewItem lvi = new ListViewItem(nwDataSet1.Customers[i].CustomerID);
                listView1.Items.Add(lvi);
                if (listView1.Groups[country] == null)//該 Country 不存在就新增
                {
                    ListViewGroup listViewGroup = listView1.Groups.Add(country, nwDataSet1.Customers[i].Country);
                    listViewGroup.Tag = 0;//國家起始人數
                    listViewGroup.Items.Add(lvi);
                }
                else
                {
                    listView1.Groups[country].Items.Add(lvi);
                }
                listView1.Groups[country].Tag = (int)listView1.Groups[country].Tag + 1;
                listView1.Groups[country].Header = $"{country} ({listView1.Groups[country].Tag})";
            }
            listView1.Visible = true;//還原顯示 ListView 
            Cursor = Cursors.Default;//還原游標圖片
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level != 0)//測試是否為主節點 Country
            {
                string nodeStr = e.Node.Text;                
                nodeStr = nodeStr.Substring(0, nodeStr.IndexOf("(") - 1);//處理節點文字到剩 City
                customersBindingSource.Filter = "City='" + nodeStr + "'";
                label1.Text = $"Customers({nodeStr}) Count : {customersBindingSource.Count}";
            }
        }

        //private void customersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        //{
        //    Validate();
        //    customersBindingSource.EndEdit();
        //    tableAdapterManager.UpdateAll(nwDataSet1);
        //}        
    }
}
