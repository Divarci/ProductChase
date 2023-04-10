using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
           // dtp.Clear();
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
            SqlCommand cmd = new SqlCommand("Select productprice from tblproducts where productid=@p1", conn.conn());
            cmd.Parameters.AddWithValue("@p1", cmbProduct.SelectedValue);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                productPrice = dr[0].ToString();
            }
            conn.conn().Close();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            int productStock = 0;
            int quantity2;
            if (int.TryParse(txtQuantity.Text, out quantity2))
            {
                quantity2 = Convert.ToInt16(txtQuantity.Text);
            }
            else
            {
                quantity2 = 0;
                txtQuantity.Text = "0";
                txtQuantity.SelectAll();
            }

            SqlCommand cmd = new SqlCommand("Select productstock from TBLPRODUCTS where productid=@p1", conn.conn());
            cmd.Parameters.AddWithValue("@p1", cmbProduct.SelectedValue);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                productStock = Convert.ToInt16(dr[0].ToString());
            }

            if (productStock <= 0 || quantity2 > productStock)
            {
                txtQuantity.Text = "0";
                txtQuantity.SelectAll();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbProduct.Text) || string.IsNullOrEmpty(cmbClient.Text) || string.IsNullOrEmpty(cmbEmployee.Text) || string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Please enter Valid Informations", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                List<string> tempList = new List<string>();
                int temp = 0;
                SqlCommand cmd1 = new SqlCommand("Select MovementId from TBLMOVEMENT", conn.conn());
                SqlDataReader dr = cmd1.ExecuteReader();
                while (dr.Read())
                {
                    tempList.Add(dr[0].ToString());
                }
                conn.conn().Close();
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i] == txtId.Text)
                    {
                        temp++;
                    }
                }
                if (temp > 0)
                {
                    MessageBox.Show("The Data Id has been used for another Data. Please clear the form and try to save a new one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("insert into TBLMOVEMENT (PRODUCT,CLIENT,EMPLOYEE,QUANTITY) VALUES (@P1,@P2,@P3,@P4)", conn.conn());
                    cmd.Parameters.AddWithValue("@p1", cmbProduct.SelectedValue);
                    cmd.Parameters.AddWithValue("@p2", cmbClient.SelectedValue);
                    cmd.Parameters.AddWithValue("@p3", cmbEmployee.SelectedValue);
                    cmd.Parameters.AddWithValue("@p4", txtQuantity.Text);
                    cmd.ExecuteNonQuery();
                    conn.conn().Close();

                    MessageBox.Show("The Data has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listIt();
                    clean();
                }


            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbProduct.Text) || string.IsNullOrEmpty(cmbClient.Text) || string.IsNullOrEmpty(cmbEmployee.Text) || string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Please enter Valid Informations", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure to UPDATE Data Id: " + txtId.Text, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {

                    SqlCommand cmd = new SqlCommand("Update TBLMOVEMENT set PRODUCT=@P1,CLIENT=@P2,EMPLOYEE=@P3,QUANTITY=@P4 where movementid=@p5", conn.conn());
                    cmd.Parameters.AddWithValue("@p1", cmbProduct.SelectedValue);
                    cmd.Parameters.AddWithValue("@p2", cmbClient.SelectedValue);
                    cmd.Parameters.AddWithValue("@p3", cmbEmployee.SelectedValue);
                    cmd.Parameters.AddWithValue("@p4", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@p5", txtId.Text);
                    cmd.ExecuteNonQuery();
                    conn.conn().Close();

                    MessageBox.Show("Data Id: " + txtId.Text+" has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listIt();
                    clean();
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int choosen = dataGridView1.SelectedCells[0].RowIndex;

            txtId.Text = dataGridView1.Rows[choosen].Cells[0].Value.ToString();
            cmbCategory.Text = dataGridView1.Rows[choosen].Cells[1].Value.ToString();
            cmbProduct.Text = dataGridView1.Rows[choosen].Cells[2].Value.ToString();
            cmbClient.Text = dataGridView1.Rows[choosen].Cells[3].Value.ToString();
            cmbEmployee.Text = dataGridView1.Rows[choosen].Cells[4].Value.ToString();
            txtQuantity.Text = dataGridView1.Rows[choosen].Cells[5].Value.ToString();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a Valid Data Id by selection from table", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure to DELETE Data Id: " + txtId.Text, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("Delete from TBLMOVEMENT where movementid=@p1", conn.conn());
                    cmd.Parameters.AddWithValue("@p1", txtId.Text);
                    cmd.ExecuteNonQuery();
                    conn.conn().Close();

                    MessageBox.Show("Data Id: " + txtId.Text + " has been DELETED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clean();
                    listIt();
                }
            }
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("MOVEMENT_SEARCH", conn.conn());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@startdate", dtpStart.Value.Date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@enddate", dtpEnd.Value.Date.ToString("yyyy-MM-dd"));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

       
        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("MOVEMENT_SEARCH", conn.conn());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@startdate", dtpStart.Value.Date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@enddate", dtpEnd.Value.Date.ToString("yyyy-MM-dd"));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
