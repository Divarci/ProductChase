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
        public string userid;
        void SettingDataGridView(DataGridView dtv)
        {
            dtv.Columns[0].Width = 50;
            dtv.Columns[1].Width = 110;
            dtv.Columns[2].Width = 110;
            dtv.Columns[3].Width = 80;
            dtv.Columns[4].Width = 85;
            dtv.Columns[5].Width = 125;
            dtv.Columns[6].Width = 95;
            dtv.Columns[7].Visible = false;
            dtv.Columns[8].Visible = false;

        }
        public void listIt()
        {
            if (cbSee.Checked == true)
            {
                SqlCommand cmd = new SqlCommand("EXECUTE LISTCLIENT", conn.conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                SettingDataGridView(dataGridView1);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("EXECUTE LIST_CLIENT_ACTIVE", conn.conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                SettingDataGridView(dataGridView1);
            }
        }

        public void clean()
        {
            txtId.Clear();
            txtClientName.Clear();
            txtClientSurname.Clear();
            cmbCity.Text = "";
            txtClientMobile.Clear();
            txtClientEmail.Clear();
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
            if (cbSet.Checked == true)
            {
                SqlCommand cmd = new SqlCommand("LISTCLIENTWITHFILTER", conn.conn());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p1", cmbCity.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                SettingDataGridView(dataGridView1);
            }
            else
            {

                SqlCommand cmd = new SqlCommand("LIST_CLIENTFILTER_ACTIVE", conn.conn());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p1", cmbCity.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                SettingDataGridView(dataGridView1);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtClientName.Text.Trim().Length == 0 || txtClientSurname.Text.Trim().Length == 0 || txtClientMobile.Text.Trim().Length == 0 || txtClientEmail.Text.Trim().Length == 0 || cmbCity.Text == "")
            {
                MessageBox.Show("Please enter Valid Informations", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                if (txtId.Text.Trim().Length == 0)
                {
                    List<string> tempList = new List<string>();
                    int temp = 0;
                    SqlCommand cmd1 = new SqlCommand("Select ClientName+' '+ClientSurname from TBLCLIENTS", conn.conn());
                    SqlDataReader dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        tempList.Add(dr[0].ToString());
                    }
                    conn.conn().Close();

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (tempList[i] == txtClientName.Text + " " + txtClientSurname.Text)
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
                        SqlCommand cmd = new SqlCommand("insert into TBLCLIENTS (CLIENTNAME,CLIENTSURNAME,CLIENTCITY,CLIENTMOBILE,CLIENTEMAIL,USERID,INACTIVE) values (@p1,@p2,@p3,@p4,@p5,@P6,'0')", conn.conn());
                        cmd.Parameters.AddWithValue("@p1", txtClientName.Text);
                        cmd.Parameters.AddWithValue("@p2", txtClientSurname.Text);
                        cmd.Parameters.AddWithValue("@p3", cmbCity.SelectedValue);
                        cmd.Parameters.AddWithValue("@p4", txtClientMobile.Text);
                        cmd.Parameters.AddWithValue("@p5", txtClientEmail.Text);
                        cmd.Parameters.AddWithValue("@p6", userid);

                        cmd.ExecuteNonQuery();
                        conn.conn().Close();

                        MessageBox.Show("The Client has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listIt();
                        clean();
                    }
                }
                else
                {
                    List<string> tempList = new List<string>();
                    int temp = 0;
                    SqlCommand cmd1 = new SqlCommand("Select ClientId from TBLCLIENTS", conn.conn());
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
                        MessageBox.Show("The Client Id has been used for another client. Please clear the form and try to save a new one", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("insert into TBLCLIENTS (CLIENTNAME,CLIENTSURNAME,CLIENTCITY,CLIENTMOBILE,CLIENTEMAIL,USERID) values (@p1,@p2,@p3,@p4,@p5,@P6)", conn.conn());
                        cmd.Parameters.AddWithValue("@p1", txtClientName.Text);
                        cmd.Parameters.AddWithValue("@p2", txtClientSurname.Text);
                        cmd.Parameters.AddWithValue("@p3", cmbCity.SelectedValue);
                        cmd.Parameters.AddWithValue("@p4", txtClientMobile.Text);
                        cmd.Parameters.AddWithValue("@p5", txtClientEmail.Text);
                        cmd.Parameters.AddWithValue("@p6", userid);

                        cmd.ExecuteNonQuery();
                        conn.conn().Close();

                        MessageBox.Show("The Client has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listIt();
                        clean();
                    }
                }

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
            if (dataGridView1.Rows[choosen].Cells[8].Value.ToString() == "True")
            {
                cbSet.Checked = true;
            }
            else { cbSet.Checked = false; }
            cmbCity.Text = dataGridView1.Rows[choosen].Cells[3].Value.ToString();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            listIt();
            clean();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtClientName.Text.Trim().Length == 0 || txtClientSurname.Text.Trim().Length == 0 || txtClientMobile.Text.Trim().Length == 0 || txtClientEmail.Text.Trim().Length == 0 || cmbCity.Text == "")
            {
                MessageBox.Show("Please enter Valid Informations", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure to UPDATE client Id: " + txtId.Text, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE TBLCLIENTS SET CLIENTNAME=@p1,CLIENTSURNAME=@p2,CLIENTCITY=@p3,CLIENTMOBILE=@p4,CLIENTEMAIL=@p5,INACTIVE = @p7 where CLIENTID=@p6", conn.conn());
                    cmd.Parameters.AddWithValue("@p1", txtClientName.Text);
                    cmd.Parameters.AddWithValue("@p2", txtClientSurname.Text);
                    cmd.Parameters.AddWithValue("@p3", cmbCity.SelectedValue);
                    cmd.Parameters.AddWithValue("@p4", txtClientMobile.Text);
                    cmd.Parameters.AddWithValue("@p5", txtClientEmail.Text);
                    cmd.Parameters.AddWithValue("@p6", txtId.Text);
                    cmd.Parameters.AddWithValue("@p7", cbSet.Checked);
                    cmd.ExecuteNonQuery();
                    conn.conn().Close();

                    MessageBox.Show("Client Id: " + txtId.Text + " has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listIt();
                    clean();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> clientIdList = new List<string>();
            SqlCommand cmd2 = new SqlCommand("select CLIENT from TBLMOVEMENT where CLIENT=@p1", conn.conn());
            cmd2.Parameters.AddWithValue("@p1", txtId.Text);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                clientIdList.Add(dr2[0].ToString());
            }
            conn.conn().Close();
            if (clientIdList.Count > 0)
            {
                MessageBox.Show("You can not delete this client as it is connected other data. Try to use INACTIVE product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (txtId.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter a Valid Client Id by selection from table", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure to DELETE client Id: " + txtId.Text, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("Delete from TBLCLIENTS where clientId=@p1", conn.conn());
                        cmd.Parameters.AddWithValue("@p1", txtId.Text);
                        cmd.ExecuteNonQuery();
                        conn.conn().Close();

                        MessageBox.Show("Client Id: " + txtId.Text + " has been DELETED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Choose a Client for information", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                MessageBox.Show(clientNameAndSurname + " : This client had been recorded by " + userNameAndSurname, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        string clientNameAndSurname, recordedUser;
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int choosen;
            choosen = dataGridView1.SelectedCells[0].RowIndex;


            clientNameAndSurname = dataGridView1.Rows[choosen].Cells[1].Value.ToString() + " " + dataGridView1.Rows[choosen].Cells[2].Value.ToString(); ;
            recordedUser = dataGridView1.Rows[choosen].Cells[7].Value.ToString();



        }
    }
}
