using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductChase
{
    public partial class frmMovement : Form
    {
        public frmMovement()
        {
            InitializeComponent();
        }
        ConnectionToSql conn = new ConnectionToSql();

        public void clean()
        {
            txtId.Clear();
            txtCost.Clear();
            txtFilter.Clear();
            txtQuantity.Clear();
            cmbCategory.Text = "";
            cmbClient.Text = "";
            cmbEmployee.Text = "";
            cmbProduct.Text = "";
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[5].Width = 90;
            
        }
        public void listIt()
        {
            SqlCommand cmd = new SqlCommand("EXECUTE LISTMOVEMENT", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void frmMovement_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Select * from TBLCATEGORY", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCategory.ValueMember = "CATEGORYID";
            cmbCategory.DisplayMember = "CATEGORYNAME";
            cmbCategory.DataSource = dt;

            SqlCommand cmd2 = new SqlCommand("Select * from TBLEMPLOYEE", conn.conn());
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            cmbEmployee.ValueMember = "EMPLOYEEID";
            cmbEmployee.DisplayMember = "EMPLOYEENAME";
            cmbEmployee.DataSource = dt2;

            SqlCommand cmd3 = new SqlCommand("Select CLIENTID,CLIENTNAME+' '+CLIENTSURNAME AS 'CLIENT' from TBLCLIENTs", conn.conn());
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            cmbClient.ValueMember = "CLIENTID";
            cmbClient.DisplayMember = "CLIENT";
            cmbClient.DataSource = dt3;

            listIt();
            clean();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Select PRODUCTID, PRODUCTNAME +'-'+PRODUCTBRAND AS 'PRODUCT',PRODUCTPRICE from TBLPRODUCTS where category = @p1", conn.conn());
            cmd.Parameters.AddWithValue("@p1", cmbCategory.SelectedValue);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbProduct.ValueMember = "PRODUCTID";
            cmbProduct.DisplayMember = "PRODUCT";
            cmbProduct.DataSource = dt;
        }

       
        string productPrice;
        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Select productprice from tblproducts where productid=@p1",conn.conn());
            cmd.Parameters.AddWithValue("@p1",cmbProduct.SelectedValue);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                productPrice = dr[0].ToString();
            }
            conn.conn().Close();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            int productStock=0;
            int quantity2 = Convert.ToInt16(txtQuantity.Text);
            SqlCommand cmd = new SqlCommand("Select productstock from TBLPRODUCTS where productid=@p1", conn.conn());
            cmd.Parameters.AddWithValue("@p1", cmbProduct.SelectedValue);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                productStock = Convert.ToInt16(dr[0].ToString());
            }
            
            if (productStock <= 0 || quantity2>productStock)
            {
                txtQuantity.Text = "0";
            }
            else
            {
                decimal quantity;
                decimal productPriceDec;
                if (decimal.TryParse(txtQuantity.Text, out quantity) && decimal.TryParse(productPrice, out productPriceDec))
                {
                    decimal cost = quantity * productPriceDec;
                    txtCost.Text = cost.ToString();
                }
            }

            
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            listIt();
            clean();
        }
    }
}
