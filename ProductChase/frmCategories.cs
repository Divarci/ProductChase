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
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }
        ConnectionToSql conn = new ConnectionToSql();
        public void listIt()
        {
            SqlCommand cmd = new SqlCommand("Select * from TBLCATEGORY", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        public void Clean()
        {
            txtId.Clear();
            txtCatergory.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> categoryNameList = new List<string>();
            SqlCommand cmd1 = new SqlCommand("Select CategoryName from TBLCATEGORY", conn.conn());
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                categoryNameList.Add(dr[0].ToString());
            }
            conn.conn().Close();
            int temp = 0;
            for (int i = 0; i < categoryNameList.Count; i++)
            {
                if (categoryNameList[i] == txtCatergory.Text)
                {
                    temp++;
                }
            }

            if (temp > 0)
            {
                MessageBox.Show("This category has already been ADDED. Please try to add different category", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SqlCommand cmd2 = new SqlCommand("insert into TBLCATEGORY (CATEGORYNAME) values (@p1)", conn.conn());
                cmd2.Parameters.AddWithValue("@p1", txtCatergory.Text);
                cmd2.ExecuteNonQuery();
                conn.conn().Close();
                MessageBox.Show("Category has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clean();
                listIt();
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            listIt();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txtId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCatergory.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("delete from TBLCATEGORY where CategoryId=@p1", conn.conn());
            cmd.Parameters.AddWithValue("@p1", txtId.Text);
            cmd.ExecuteNonQuery();
            conn.conn().Close();

            MessageBox.Show("Category has been DELETED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clean();
            listIt();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("Update TBLCATEGORY set CategoryName=@p1 where categoryid=@p2",conn.conn());
            cmd.Parameters.AddWithValue("@p1", txtCatergory.Text);
            cmd.Parameters.AddWithValue("@p2", txtId.Text);
            cmd.ExecuteNonQuery();
            conn.conn().Close();

            MessageBox.Show("Category has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clean();
            listIt();

        }
    }
}
