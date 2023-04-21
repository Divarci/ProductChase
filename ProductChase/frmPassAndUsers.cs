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
    public partial class frmPassAndUsers : Form
    {
        public frmPassAndUsers()
        {
            InitializeComponent();
        }
        ConnectionToSql conn = new ConnectionToSql();
        public string userid;

        private void btnCancel_Click(object sender, EventArgs e)
        {   
            frmMainMenu fr = new frmMainMenu();
            fr.newInfo = txtUN3 + " " + txtUN4;
            fr.userid = userid;
            fr.Show();
            this.Close();
        }

        string UN, UN2;

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            byte[] uNcyropted = ASCIIEncoding.ASCII.GetBytes(txtUN.Text);
            string uNcyropted2 = Convert.ToBase64String(uNcyropted);

            byte[] uNcyropted3 = ASCIIEncoding.ASCII.GetBytes(txtUN2.Text);
            string uNcyropted4 = Convert.ToBase64String(uNcyropted3);


            SqlCommand cmd = new SqlCommand("update TBLUSERS SET username=@p1,pass=@p2,name=@p3,surname=@p4 where userid=@p5", conn.conn());
            cmd.Parameters.AddWithValue("@p1", uNcyropted2);
            cmd.Parameters.AddWithValue("@p2", uNcyropted4);
            cmd.Parameters.AddWithValue("@p3", txtUN3.Text);
            cmd.Parameters.AddWithValue("@p4", txtUN4.Text);
            cmd.Parameters.AddWithValue("@p5", userid);
            cmd.ExecuteNonQuery();
            conn.conn().Close();
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            txtUN2.UseSystemPasswordChar = false;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            txtUN2.UseSystemPasswordChar = true;
        }

        private void frmPassAndUsers_Load(object sender, EventArgs e)
        {
            SqlCommand name = new SqlCommand("Select * from TBLUSERS where USERID=@p1", conn.conn());
            name.Parameters.AddWithValue("@p1", userid);
            SqlDataReader drname = name.ExecuteReader();
            while (drname.Read())
            {
                UN = drname[1].ToString();
                UN2 = drname[2].ToString();
                txtUN3.Text = drname[3].ToString();
                txtUN4.Text = drname[4].ToString();
            }
            conn.conn().Close();

            byte[] uNcyrpto = Convert.FromBase64String(UN);
            string original = ASCIIEncoding.ASCII.GetString(uNcyrpto);

            byte[] uNcyrpto2 = Convert.FromBase64String(UN2);
            string original2 = ASCIIEncoding.ASCII.GetString(uNcyrpto2);

            txtUN.Text = original;
            txtUN2.Text = original2;

        }





    }
}
