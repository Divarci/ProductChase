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
    public partial class frmEmployee : Form
    {
        public frmEmployee()
        {
            InitializeComponent();
        }
        public string userid;

        ConnectionToSql conn = new ConnectionToSql();
        public void listIt()
        {
            if (cbSee.Checked)
            {
                SqlCommand cmd = new SqlCommand("LIST_EMOLOYEE", conn.conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
            }
            else
            {
                SqlCommand cmd = new SqlCommand("LIST_EMOLOYEE_ACTIVE", conn.conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
            }
            
        }

        public void Clean()
        {
            txtId.Clear();
            txtEmployeeName.Clear();
        }
        private void frmEmployee_Load(object sender, EventArgs e)
        {
            listIt();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtEmployeeName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a Valid Emloyee Name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                List<string> employeeNameList = new List<string>();
                SqlCommand cmd1 = new SqlCommand("Select employeename from TBLEMPLOYEE", conn.conn());
                SqlDataReader dr = cmd1.ExecuteReader();
                while (dr.Read())
                {
                    employeeNameList.Add(dr[0].ToString());
                }
                conn.conn().Close();
                int temp = 0;
                for (int i = 0; i < employeeNameList.Count; i++)
                {
                    if (employeeNameList[i] == txtEmployeeName.Text)
                    {
                        temp++;
                    }
                }

                if (temp > 0)
                {
                    MessageBox.Show("This employee has already been ADDED. Please try to add different employee", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand cmd2 = new SqlCommand("insert into TBLEMPLOYEE (employeename,userid,inactive) values (@p1,@p2,@p3)", conn.conn());
                    cmd2.Parameters.AddWithValue("@p1", txtEmployeeName.Text);
                    cmd2.Parameters.AddWithValue("@p2", userid);
                    cmd2.Parameters.AddWithValue("@p3", cbSet);
                    cmd2.ExecuteNonQuery();
                    conn.conn().Close();
                    MessageBox.Show("Employee has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clean();
                    listIt();
                }
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtEmployeeName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a Valid Emloyee Name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure to UPDATE Id Number: " + txtId.Text + " Employee", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("Update TBLEMPLOYEE set employeename=@p1,inactive=@p3 where employeeid=@p2", conn.conn());
                    cmd.Parameters.AddWithValue("@p1", txtEmployeeName.Text);
                    cmd.Parameters.AddWithValue("@p2", txtId.Text);
                    cmd.Parameters.AddWithValue("@p3", cbSet.Checked);
                    cmd.ExecuteNonQuery();
                    conn.conn().Close();

                    MessageBox.Show("Id Number: " + txtId.Text + " Employee Information has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clean();
                    listIt();
                }
                
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> employeeIdList = new List<string>();
            SqlCommand cmd2 = new SqlCommand("Select EMPLOYEE from TBLMOVEMENT WHERE EMPLOYEE=@P1", conn.conn());
            cmd2.Parameters.AddWithValue("@p1", txtId.Text);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                employeeIdList.Add(dr2[0].ToString());
            }
            conn.conn().Close();
            if (employeeIdList.Count > 0)
            {
                MessageBox.Show("You can not delete this employee as it is connected other data. Try to use INACTIVE product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (txtId.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter a Valid Emloyee Id by selection from table", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure to DELETE Id Number: " + txtId.Text + " Employee", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("delete from TBLEMPLOYEE where employeeid=@p1", conn.conn());
                        cmd.Parameters.AddWithValue("@p1", txtId.Text);
                        cmd.ExecuteNonQuery();
                        conn.conn().Close();

                        MessageBox.Show("Id Number: " + txtId.Text + " Employee has been DELETED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clean();
                        listIt();
                    }
                }
            }

           
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            listIt();
            Clean();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txtId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtEmployeeName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() == "True")
            {
                cbSet.Checked = true;
            }
            else { cbSet.Checked = false; }
        }

        string userNameAndSurname;

        private void recordInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (recordedUser == string.Empty || recordedUser == "" || recordedUser == null)
            {
                MessageBox.Show("Choose an Employee for information", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                MessageBox.Show(employeeName + " :This employee had been recorded by " + userNameAndSurname, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        string employeeName, recordedUser;
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int choosen = dataGridView1.SelectedCells[0].RowIndex;

            employeeName = dataGridView1.Rows[choosen].Cells[1].Value.ToString();
            recordedUser = dataGridView1.Rows[choosen].Cells[2].Value.ToString();
        }
    }
}
