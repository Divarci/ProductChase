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
    public partial class frmClient : Form
    {
        public frmClient()
        {
            InitializeComponent();
        }
        ConnectionToSql conn = new ConnectionToSql();

        public void listIt()
        {
            SqlCommand cmd = new SqlCommand("EXECUTE LISTCLIENT", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void frmClient_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Select * from TBLCITIES", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCity.ValueMember = "CITYID";
            cmbCity.DisplayMember = "CITYNAME";
            cmbCity.DataSource = dt;
            cmbCity.Text = string.Empty;
            listIt();
        }

        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("LISTCLIENTWITHFILTER", conn.conn());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p1", cmbCity.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> clientNameList = new List<string>();
            SqlCommand cmd1 = new SqlCommand("Select ClientName+' '+ClientSurname from TBLCLIENTS", conn.conn());
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                clientNameList.Add(dr[0].ToString());
            }
            conn.conn().Close();
            int temp = 0;
            for (int i = 0; i < clientNameList.Count; i++)
            {
                if (clientNameList[i] == txtClientName.Text + " " + txtClientSurname.Text)
                {
                    temp++;
                }
            }

            if (temp > 0)
            {
                MessageBox.Show("This client has already been ADDED. Please try to add different client", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("insert into TBLCLIENTS (CLIENTNAME,CLIENTSURNAME,CLIENTCITY,CLIENTMOBILE,CLIENTEMAIL) values (@p1,@p2,@p3,@p4,@p5)", conn.conn());
                cmd.Parameters.AddWithValue("@p1", txtClientName.Text);
                cmd.Parameters.AddWithValue("@p2", txtClientSurname.Text);
                cmd.Parameters.AddWithValue("@p3", cmbCity.SelectedValue);
                cmd.Parameters.AddWithValue("@p4", txtClientMobile.Text);
                cmd.Parameters.AddWithValue("@p5", txtClientEmail.Text);

                cmd.ExecuteNonQuery();
                conn.conn().Close();

                MessageBox.Show("The Client has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listIt();
                // Clean();
            }

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int choosen;
            choosen = dataGridView1.SelectedCells[0].RowIndex;

            txtId.Text = dataGridView1.Rows[choosen].Cells[0].Value.ToString();
            txtClientName.Text = dataGridView1.Rows[choosen].Cells[1].Value.ToString();
            txtClientSurname.Text = dataGridView1.Rows[choosen].Cells[2].Value.ToString();
            txtClientMobile.Text = dataGridView1.Rows[choosen].Cells[4].Value.ToString();
            txtClientEmail.Text = dataGridView1.Rows[choosen].Cells[5].Value.ToString();
            cmbCity.Text = dataGridView1.Rows[choosen].Cells[3].Value.ToString();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            listIt();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("UPDATE TBLCLIENTS SET CLIENTNAME=@p1,CLIENTSURNAME=@p2,CLIENTCITY=@p3,CLIENTMOBILE=@p4,CLIENTEMAIL=@p5 where CLIENTID=@p6", conn.conn());
            cmd.Parameters.AddWithValue("@p1", txtClientName.Text);
            cmd.Parameters.AddWithValue("@p2", txtClientSurname.Text);
            cmd.Parameters.AddWithValue("@p3", cmbCity.SelectedValue);
            cmd.Parameters.AddWithValue("@p4", txtClientMobile.Text);
            cmd.Parameters.AddWithValue("@p5", txtClientEmail.Text);
            cmd.Parameters.AddWithValue("@p6", txtId.Text);
            cmd.ExecuteNonQuery();
            conn.conn().Close();

            MessageBox.Show("The Client has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listIt();
            //Clean();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Delete from TBLCLIENTS where clientId=@p1", conn.conn());
            cmd.Parameters.AddWithValue("@p1", txtId.Text);
            cmd.ExecuteNonQuery();
            conn.conn().Close();

            MessageBox.Show("Client has been DELETED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //clean();
            listIt();
        }
    }
}
