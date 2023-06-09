﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        //sql connection
        ConnectionToSql conn = new ConnectionToSql();

        public string userid;

        //list method
        public void listIt()
        {
            //checks active and inactive status
            if (cbSee.Checked)
            {
                //all categories
                SqlCommand cmd = new SqlCommand("EXECUTE LIST_CATEGORY", conn.conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
            }
            else
            {
                //just active ones
                SqlCommand cmd = new SqlCommand("EXECUTE LIST_CATEGORY_WITH_ACTIVE", conn.conn());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
            }
          
            
        }
        //clean method
        public void Clean()
        {
            txtId.Clear();
            txtCatergory.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //checks if all forms are filled
            if (txtCatergory.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a Valid Category Name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //check if the category name that is written was in database or not
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
                    MessageBox.Show("This category has already been ADDED. Please try to add different category", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //if it is not
                else
                {
                    SqlCommand cmd2 = new SqlCommand("insert into TBLCATEGORY (CATEGORYNAME,USERID,INACTIVE) values (@p1,@P2,@p3)", conn.conn());
                    cmd2.Parameters.AddWithValue("@p1", txtCatergory.Text);
                    cmd2.Parameters.AddWithValue("@p2", userid);
                    cmd2.Parameters.AddWithValue("@p3", cbSet.Checked);
                    cmd2.ExecuteNonQuery();
                    conn.conn().Close();
                    MessageBox.Show("Category has been ADDED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clean();
                    listIt();
                }
            }
        }
        //listing
        private void btnList_Click(object sender, EventArgs e)
        {
            listIt();
            Clean();
        }
        //double click on table action
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txtId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCatergory.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() == "True")
            {
                cbSet.Checked = true;
            }
            else { cbSet.Checked = false; }
        }
        //delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if a category is connected a data you cannot delete it.
            List<string> categoryIdList = new List<string>();
            SqlCommand cmd2 = new SqlCommand("Select CATEGORY from TBLPRODUCTS WHERE CATEGORY=@P1", conn.conn());
            cmd2.Parameters.AddWithValue("@p1", txtId.Text);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                categoryIdList.Add(dr2[0].ToString());
            }
            conn.conn().Close();
            if (categoryIdList.Count > 0)
            {
                MessageBox.Show("You can not delete this category as it is connected other data. Try to use INACTIVE product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //if it is not
            else
            {
                //check all infos were written or not
                if (txtId.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter a Valid Category Id by selection from table", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //if it is
                else
                {
                    //ask to be sure
                    DialogResult result = MessageBox.Show("Are you sure to DELETE category Id: " + txtId.Text, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    //delete
                    if (result == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("delete from TBLCATEGORY where CategoryId=@p1", conn.conn());
                        cmd.Parameters.AddWithValue("@p1", txtId.Text);
                        cmd.ExecuteNonQuery();
                        conn.conn().Close();

                        MessageBox.Show("Category Id: " + txtId.Text + " has been DELETED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clean();
                        listIt();
                    }
                }
            }

           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //checks all infos were filed or not
            if (txtCatergory.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a Valid Category Name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure to UPDATE category Id: " + txtId.Text, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                //if it is not
                if (result == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("Update TBLCATEGORY set CategoryName=@p1,INACTIVE=@p3 where categoryid=@p2", conn.conn());
                    cmd.Parameters.AddWithValue("@p1", txtCatergory.Text);
                    cmd.Parameters.AddWithValue("@p2", txtId.Text);
                    cmd.Parameters.AddWithValue("@p3", cbSet.Checked);
                    cmd.ExecuteNonQuery();
                    conn.conn().Close();

                    MessageBox.Show("Category Id: " + txtId.Text + " has been UPDATED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clean();
                    listIt();
                }
            }
        }
        //list
        private void frmCategories_Load(object sender, EventArgs e)
        {
            listIt();
        }
        string userNameAndSurname;

        //strip menu right click 
        private void recordInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if there is a userid
            if (recordedUser == string.Empty || recordedUser == "" || recordedUser == null)
            {
                MessageBox.Show("Choose a category for information", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //pulls user name and surname by userid which is came from single click on tanle
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

                MessageBox.Show(category + " category had been recorded by " + userNameAndSurname, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        // temporary objects keep values
        string category, recordedUser;

        //single click on table
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int choosen = dataGridView1.SelectedCells[0].RowIndex;

            category = dataGridView1.Rows[choosen].Cells[1].Value.ToString();
            recordedUser = dataGridView1.Rows[choosen].Cells[2].Value.ToString();
        }

    }
}
