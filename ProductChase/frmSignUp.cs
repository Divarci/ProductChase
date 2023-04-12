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
    public partial class frmSignUp : Form
    {
        public frmSignUp()
        {
            InitializeComponent();
        }
        ConnectionToSql conn = new ConnectionToSql();

        void clean()
        {
            txtName.Clear();
            txtName2.Clear();
            txtName3.Clear();
            txtSurname.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            frmLogin fr = new frmLogin();
            fr.Show();
            this.Close();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtName2.Text == "" || txtName3.Text == "" || txtSurname.Text == "")
            {
                MessageBox.Show("Please provide all informations", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                byte[] name = ASCIIEncoding.ASCII.GetBytes(txtName2.Text);
                string named = Convert.ToBase64String(name);

                byte[] name2 = ASCIIEncoding.ASCII.GetBytes(txtName3.Text);
                string named2 = Convert.ToBase64String(name2);

                SqlCommand cmd = new SqlCommand("Insert into TBLUSERS (USERNAME,PASS,NAME,SURNAME) values (@p1,@p2,@p3,@p4) ", conn.conn());
                cmd.Parameters.AddWithValue("@p1", named);
                cmd.Parameters.AddWithValue("@p2", named2);
                cmd.Parameters.AddWithValue("@p3", txtName.Text);
                cmd.Parameters.AddWithValue("@p4", txtSurname.Text);
                cmd.ExecuteNonQuery();
                conn.conn().Close();

                MessageBox.Show("User has been Saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clean();
            }


        }


    }
}
