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

        public string userid;

        private void listIt()
        {

            if (cbSee.Checked == true)
            {
                SqlCommand cmd = new SqlCommand("EXECUTE LISTPRODUCTS", conn.conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                SettingDataGridView(dataGridView1);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("EXECUTE LIST_PRODUCTS_ACTIVE", conn.conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                SettingDataGridView(dataGridView1);
            }

        }
        private void clean()
        {
            txtId.Clear();
            txtProductName.Clear();
            txtBrand.Clear();
            txtCost.Clear();
            txtPrice.Clear();
            txtStock.Clear();


        }

        void SettingDataGridView(DataGridView dtv)
        {
            dtv.Columns[0].Width = 60;
            dtv.Columns[1].Width = 250;
            dtv.Columns[2].Width = 100;
            dtv.Columns[3].Width = 120;
            dtv.Columns[4].Width = 80;
            dtv.Columns[5].Width = 80;
            dtv.Columns[6].Width = 60;
            dtv.Columns[8].Visible = false;
            dtv.Columns[9].Visible = false;

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
            listIt();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text.Trim().Length == 0 || txtBrand.Text.Trim().Length == 0 || txtCost.Text.Trim().Length == 0 || txtPrice.Text.Trim().Length == 0 || txtStock.Text.Trim().Length == 0 || cbmCategory.Text == "")
            {
                MessageBox.Show("Please enter Valid Informations", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                if (txtId.Text.Trim().Length == 0)
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
                        try
                        {
                            SqlCommand cmd = new SqlCommand("insert into TBLPRODUCTS (PRODUCTNAME,PRODUCTBRAND,CATEGORY,PRODUCTCOST,PRODUCTPRICE,PRODUCTSTOCK,USERID,INACTIVE) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,'0')", conn.conn());
                            cmd.Parameters.AddWithValue("@p1", txtProductName.Text);
                            cmd.Parameters.AddWithValue("@p2", txtBrand.Text);
                            cmd.Parameters.AddWithValue("@p3", cbmCategory.SelectedValue);
                            cmd.Parameters.AddWithValue("@p4", txtCost.Text);
                            cmd.Parameters.AddWithValue("@p5", txtPrice.Text);
                            cmd.Parameters.AddWithValue("@p6", txtStock.Text);
                            cmd.Parameters.AddWithValue("@p7", userid);
                            cmd.ExecuteNonQuery();
                            conn.conn().Close();
                            MessageBox.Show("The Product has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            listIt();
                            clean();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Please check the information you have entered.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }



                    }
                }
                else
                {
                    List<string> productIdList = new List<string>();
                    SqlCommand cmd1 = new SqlCommand("Select ProductId from TBLPRODUCTS", conn.conn());
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        productIdList.Add(dr[0].ToString());
                    }
                    conn.conn().Close();
                    int temp = 0;
                    for (int i = 0; i < productIdList.Count; i++)
                    {
                        if (productIdList[i] == txtId.Text)
                        {
                            temp++;
                        }
                    }

                    if (temp > 0)
                    {
                        MessageBox.Show("The Product Id has been used for another product. Please clear the form and try to save a new one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        try
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
                        catch (Exception)
                        {

                            MessageBox.Show("Please check the information you have entered.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }





                    }

                }
            }
        }

        private void cbmCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbSee.Checked == true)
            {
                SqlCommand cmd = new SqlCommand("LISTPRODUCTSWITHFILTER", conn.conn());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p1", cbmCategory.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[8].Visible = false;
            }
            else
            {
                SqlCommand cmd = new SqlCommand("LIST_PRODUCTFILTER_WITH_ACTIVE", conn.conn());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p1", cbmCategory.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[8].Visible = false;
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text.Trim().Length == 0 || txtBrand.Text.Trim().Length == 0 || txtCost.Text.Trim().Length == 0 || txtPrice.Text.Trim().Length == 0 || txtStock.Text.Trim().Length == 0 || cbmCategory.Text == "")
            {
                MessageBox.Show("Please enter Valid Informations", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure to UPDATE product Id: " + txtId.Text, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE TBLPRODUCTS SET PRODUCTNAME=@p1,PRODUCTBRAND=@p2,CATEGORY=@p3,PRODUCTCOST=@p4,PRODUCTPRICE=@p5,PRODUCTSTOCK=@p6,INACTIVE=@p8 where productid=@p7", conn.conn());
                    cmd.Parameters.AddWithValue("@p1", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@p2", txtBrand.Text);
                    cmd.Parameters.AddWithValue("@p3", cbmCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@p4", txtCost.Text);
                    cmd.Parameters.AddWithValue("@p5", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@p6", txtStock.Text);
                    cmd.Parameters.AddWithValue("@p7", txtId.Text);
                    cmd.Parameters.AddWithValue("@p8", cbSet.Checked);
                    cmd.ExecuteNonQuery();
                    conn.conn().Close();

                    MessageBox.Show("product Id: " + txtId.Text + " has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listIt();
                    clean();
                }
            }
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
            if (dataGridView1.Rows[choosen].Cells[9].Value.ToString() == "True")
            {
                cbSet.Checked = true;
            }
            else { cbSet.Checked = false; }
            cbmCategory.Text = dataGridView1.Rows[choosen].Cells[3].Value.ToString();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> productIdList = new List<string>();
            SqlCommand cmd2 = new SqlCommand("Select PRODUCT from TBLMOVEMENT WHERE PRODUCT=@P1", conn.conn());
            cmd2.Parameters.AddWithValue("@p1", txtId.Text);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                productIdList.Add(dr2[0].ToString());
            }
            conn.conn().Close();
            if (productIdList.Count > 0)
            {
                MessageBox.Show("You can not delete this product as it is connected other data. Try to use INACTIVE product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                if (txtId.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter a Valid Product Id by selection from table", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure to DELETE product Id: " + txtId.Text, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("Delete from TBLPRODUCTS where productId=@p1", conn.conn());
                        cmd.Parameters.AddWithValue("@p1", txtId.Text);
                        cmd.ExecuteNonQuery();
                        conn.conn().Close();

                        MessageBox.Show("Product Id: " + txtId.Text + " has been DELETED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clean();
                        listIt();
                    }
                }
            }

        }

        string userNameAndSurname;

        private void recordInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (recordedUser == string.Empty || recordedUser == "" || recordedUser == null)
            {
                MessageBox.Show("Choose a Product for information", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT NAME,SURNAME FROM TBLUSERS WHERE USERID=@P1", conn.conn());
                cmd.Parameters.AddWithValue("@P1", recordedUser);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    userNameAndSurname = dr[0].ToString() + " " + dr[1].ToString();
                }
                conn.conn().Close();

                MessageBox.Show(productName + " :This Product had been recorded by " + userNameAndSurname, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        string productName, recordedUser;
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int choosen = dataGridView1.SelectedCells[0].RowIndex;

            productName = dataGridView1.Rows[choosen].Cells[1].Value.ToString();
            recordedUser = dataGridView1.Rows[choosen].Cells[8].Value.ToString();



        }
    }
}
