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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }


        //sql connection class
        ConnectionToSql conn = new ConnectionToSql();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //username and pass cyrpted
            byte[] name = ASCIIEncoding.ASCII.GetBytes(txtName2.Text);
            string named = Convert.ToBase64String(name);

            byte[] name2 = ASCIIEncoding.ASCII.GetBytes(txtName3.Text);
            string named2 = Convert.ToBase64String(name2);

            //username and pass control
            SqlCommand cmd = new SqlCommand("Select * from TBLUSERS where USERNAME=@p1 and PASS=@P2", conn.conn());
            cmd.Parameters.AddWithValue("@p1", named);
            cmd.Parameters.AddWithValue("@p2", named2);
            SqlDataReader dr = cmd.ExecuteReader();
            //if its match
            if (dr.Read())
            {
                named = dr[1].ToString();
                named2 = dr[2].ToString();

                frmMainMenu fr = new frmMainMenu();
                fr.userid = dr[0].ToString();
                fr.Show();
                this.Hide();
            }
            //if its not match 
            else
            {
                MessageBox.Show("Please provide valid information", "Id or Password Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName2.Text = "";
                txtName3.Text = "";
                txtName2.Focus();
            }


        }

        //pass protect cancel
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            txtName3.UseSystemPasswordChar = false;
        }
        //pass protect activate
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            txtName3.UseSystemPasswordChar = true;
        }
        //signup page
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSignUp fr = new frmSignUp();
            fr.Show();
            this.Hide();
        }
        //exit app
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
      

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtName2.Focus();
        }
    }
}
