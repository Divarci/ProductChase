using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductChase
{
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }

        ConnectionToSql conn = new ConnectionToSql();

        private void listIt()
        {
            SqlCommand cmd = new SqlCommand("EXECUTE LISTPRODUCTS", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void clean()
        {
            txtProductName.Clear();
            txtBrand.Clear();
            txtCost.Clear();
            txtPrice.Clear();
            txtStock.Clear();
            cbmCategory.Text = string.Empty;

        }
        private void btnList_Click(object sender, EventArgs e)
        {
            listIt();
            clean();
        }



        private void frmProducts_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Select * from TBLCATEGORY", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbmCategory.ValueMember = "CATEGORYID";
            cbmCategory.DisplayMember = "CATEGORYNAME";
            cbmCategory.DataSource = dt;
            cbmCategory.Text = string.Empty;
            listIt();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> productNameList = new List<string>();
            SqlCommand cmd1 = new SqlCommand("Select ProductName from TBLPRODUCTS", conn.conn());
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                productNameList.Add(dr[0].ToString());
            }
            conn.conn().Close();
            int temp = 0;
            for (int i = 0; i < productNameList.Count; i++)
            {
                if (productNameList[i] == txtProductName.Text)
                {
                    temp++;
                }
            }

            if (temp > 0)
            {
                MessageBox.Show("This product has already been ADDED. Please try to add different product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("insert into TBLPRODUCTS (PRODUCTNAME,PRODUCTBRAND,CATEGORY,PRODUCTCOST,PRODUCTPRICE,PRODUCTSTOCK) values (@p1,@p2,@p3,@p4,@p5,@p6)", conn.conn());
                cmd.Parameters.AddWithValue("@p1", txtProductName.Text);
                cmd.Parameters.AddWithValue("@p2", txtBrand.Text);
                cmd.Parameters.AddWithValue("@p3", cbmCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@p4", txtCost.Text);
                cmd.Parameters.AddWithValue("@p5", txtPrice.Text);
                cmd.Parameters.AddWithValue("@p6", txtStock.Text);
                cmd.ExecuteNonQuery();
                conn.conn().Close();

                MessageBox.Show("The Product has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listIt();
                clean();
            }

        }

        private void cbmCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("LISTPRODUCTSWITHFILTER", conn.conn());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p1", cbmCategory.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("UPDATE TBLPRODUCTS SET PRODUCTNAME=@p1,PRODUCTBRAND=@p2,CATEGORY=@p3,PRODUCTCOST=@p4,PRODUCTPRICE=@p5,PRODUCTSTOCK=@p6 where productid=@p7", conn.conn());
            cmd.Parameters.AddWithValue("@p1", txtProductName.Text);
            cmd.Parameters.AddWithValue("@p2", txtBrand.Text);
            cmd.Parameters.AddWithValue("@p3", cbmCategory.SelectedValue);
            cmd.Parameters.AddWithValue("@p4", txtCost.Text);
            cmd.Parameters.AddWithValue("@p5", txtPrice.Text);
            cmd.Parameters.AddWithValue("@p6", txtStock.Text);
            cmd.Parameters.AddWithValue("@p7", txtId.Text);
            cmd.ExecuteNonQuery();
            conn.conn().Close();

            MessageBox.Show("The Product has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listIt();
            clean();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string costPriceWithCurrency;
            string priceWithCurrency;

            int choosen = dataGridView1.SelectedCells[0].RowIndex;
            txtId.Text = dataGridView1.Rows[choosen].Cells[0].Value.ToString();
            txtProductName.Text = dataGridView1.Rows[choosen].Cells[1].Value.ToString();
            txtBrand.Text = dataGridView1.Rows[choosen].Cells[2].Value.ToString();
           
            costPriceWithCurrency = dataGridView1.Rows[choosen].Cells[4].Value.ToString();
            txtCost.Text = costPriceWithCurrency.Replace("£", "");

            priceWithCurrency = dataGridView1.Rows[choosen].Cells[5].Value.ToString();
            txtPrice.Text = priceWithCurrency.Replace("£", "");

            txtStock.Text = dataGridView1.Rows[choosen].Cells[6].Value.ToString();
            cbmCategory.Text = dataGridView1.Rows[choosen].Cells[3].Value.ToString();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Delete from TBLPRODUCTS where productId=@p1", conn.conn());
            cmd.Parameters.AddWithValue("@p1",txtId.Text);
            cmd.ExecuteNonQuery();
            conn.conn().Close();

            MessageBox.Show("Product has been DELETED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            clean();
            listIt();
        }
    }
}
