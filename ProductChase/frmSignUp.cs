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
            byte[] name3 = ASCIIEncoding.ASCII.GetBytes(txtName2.Text);
            string named3 = Convert.ToBase64String(name3);

            SqlCommand cmd2 = new SqlCommand("select USERNAME from TBLUSERS", conn.conn());
            SqlDataReader dr = cmd2.ExecuteReader();

            List<string> userNameCollection = new List<string>();
            int sameUserNameReader = 0;

            while (dr.Read())
            {
                userNameCollection.Add(dr[0].ToString());
            }
            conn.conn().Close();

            foreach (var item in userNameCollection)
            {
                if (named3 == item)
                {
                    sameUserNameReader++;
                }
            }

            if (txtName.Text == "" || txtName2.Text == "" || txtName3.Text == "" || txtSurname.Text == "")
            {
                MessageBox.Show("Please provide all informations", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (sameUserNameReader > 0)
            {
                MessageBox.Show("This username has already been taken bu another user.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            txtName3.UseSystemPasswordChar = false;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            txtName3.UseSystemPasswordChar = true;

        }

        private void frmSignUp_Load(object sender, EventArgs e)
        {

        }
    }
}
