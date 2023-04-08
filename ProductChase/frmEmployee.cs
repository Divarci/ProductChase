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

        ConnectionToSql conn = new ConnectionToSql();
        public void listIt()
        {
            SqlCommand cmd = new SqlCommand("Select * from TBLEMPLOYEE", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
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
                SqlCommand cmd2 = new SqlCommand("insert into TBLEMPLOYEE (employeename) values (@p1)", conn.conn());
                cmd2.Parameters.AddWithValue("@p1", txtEmployeeName.Text);
                cmd2.ExecuteNonQuery();
                conn.conn().Close();
                MessageBox.Show("Employee has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clean();
                listIt();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Update TBLEMPLOYEE set employeename=@p1 where employeeid=@p2", conn.conn());
            cmd.Parameters.AddWithValue("@p1", txtEmployeeName.Text);
            cmd.Parameters.AddWithValue("@p2", txtId.Text);
            cmd.ExecuteNonQuery();
            conn.conn().Close();

            MessageBox.Show("Employee Information has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clean();
            listIt();
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("delete from TBLEMPLOYEE where employeeid=@p1", conn.conn());
            cmd.Parameters.AddWithValue("@p1", txtId.Text);
            cmd.ExecuteNonQuery();
            conn.conn().Close();

            MessageBox.Show("Employee has been DELETED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clean();
            listIt();
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
        }
    }
}
