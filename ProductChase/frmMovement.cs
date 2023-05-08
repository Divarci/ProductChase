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

        public string userid;
        public void clean()
        {
            txtId.Clear();
            txtCost.Clear();
            txtQuantity.Clear();
            cmbClient.Text = "";
            cmbEmployee.Text = "";
            cmbProduct.Text = "";
            

        }
        void SettingDataGridView(DataGridView dtv)
        {
            dtv.Columns[0].Width = 30;
            dtv.Columns[2].Width = 80;
            dtv.Columns[6].Width = 40;
            dtv.Columns[7].Width = 90;
            dtv.Columns[8].Width = 90;
        }
        public void listIt()
        {
            SqlCommand cmd = new SqlCommand("EXECUTE LISTMOVEMENT", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[9].Visible = false;

            SettingDataGridView(dataGridView1 );
            
        }

        private void frmMovement_Load(object sender, EventArgs e)
        {
            // all comboboxes filled with needed infos
            SqlCommand cmd1 = new SqlCommand("Select PRODUCTID,PRODUCTNAME from TBLPRODUCTS", conn.conn());
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            cmbProduct.ValueMember = "PRODUCTID";
            cmbProduct.DisplayMember = "PRODUCTNAME";
            cmbProduct.DataSource = dt1;

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

        //when we select a product, its price assigned here
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

            //checks is entered quantity is a number or else
            //if it is number, makes txtquantity int and assign it to quantity2
            if (int.TryParse(txtQuantity.Text, out quantity2))
            {
                quantity2 = Convert.ToInt16(txtQuantity.Text);
            }
            //if it is not makes it 0
            else
            {
                quantity2 = 0;
                txtQuantity.Text = "0";
                txtQuantity.SelectAll();
            }

            //makes a stok control
            SqlCommand cmd = new SqlCommand("Select productstock from TBLPRODUCTS where productid=@p1", conn.conn());
            cmd.Parameters.AddWithValue("@p1", cmbProduct.SelectedValue);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                productStock = Convert.ToInt16(dr[0].ToString());
            }

            //if stock is 0 or needed quantity more than how many we have makes quantity 0
            if (productStock <= 0 || quantity2 > productStock)
            {
                txtQuantity.Text = "0";
                txtQuantity.SelectAll();
            }
            //if it is not makes a calculation quantity x price and assign it needed form
            else
            {
                //we already have quantity
                decimal quantity;
                //we already have price just will be assigned to a decimal
                decimal productPriceDec;
                if (decimal.TryParse(txtQuantity.Text, out quantity) && decimal.TryParse(productPrice, out productPriceDec))
                {
                    decimal cost = quantity * productPriceDec;
                    txtCost.Text = cost.ToString();
                }
            }


        }
        //-----same stepss ----
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
            else if (txtQuantity.Text == "0")
            {
                MessageBox.Show("Qintity can not be 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    SqlCommand cmd = new SqlCommand("insert into TBLMOVEMENT (PRODUCT,CLIENT,EMPLOYEE,QUANTITY,userid) VALUES (@P1,@P2,@P3,@P4,@p5)", conn.conn());
                    cmd.Parameters.AddWithValue("@p1", cmbProduct.SelectedValue);
                    cmd.Parameters.AddWithValue("@p2", cmbClient.SelectedValue);
                    cmd.Parameters.AddWithValue("@p3", cmbEmployee.SelectedValue);
                    cmd.Parameters.AddWithValue("@p4", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@p5", userid);
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

                    MessageBox.Show("Data Id: " + txtId.Text + " has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listIt();
                    clean();
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int choosen = dataGridView1.SelectedCells[0].RowIndex;

            txtId.Text = dataGridView1.Rows[choosen].Cells[0].Value.ToString();
            cmbProduct.Text = dataGridView1.Rows[choosen].Cells[1].Value.ToString();
            cmbClient.Text = dataGridView1.Rows[choosen].Cells[4].Value.ToString();
            cmbEmployee.Text = dataGridView1.Rows[choosen].Cells[5].Value.ToString();
            txtQuantity.Text = dataGridView1.Rows[choosen].Cells[6].Value.ToString();

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
        //-----same stepss ----



        // makes a filter according to dates
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
            dataGridView1.Columns[9].Visible = false;
            SettingDataGridView(dataGridView1);
        }

        // makes a filter according to dates

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
            dataGridView1.Columns[9].Visible = false;
            SettingDataGridView(dataGridView1);
        }
        string userNameAndSurname;
        //same step with others
        private void recordInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (recordedUser == string.Empty || recordedUser == "" || recordedUser == null)
            {
                MessageBox.Show("Choose a Record for information", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                MessageBox.Show(movementId + " :This data had been recorded by " + userNameAndSurname, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        string movementId, recordedUser;
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int choosen = dataGridView1.SelectedCells[0].RowIndex;

            movementId = dataGridView1.Rows[choosen].Cells[0].Value.ToString();
            recordedUser = dataGridView1.Rows[choosen].Cells[9].Value.ToString();
        }
    }
}
