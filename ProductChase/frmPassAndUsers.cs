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
using System.Xml.Linq;

namespace ProductChase
{
    public partial class frmPassAndUsers : Form
    {
        public frmPassAndUsers()
        {
            InitializeComponent();
        }
        //sql connection
        ConnectionToSql conn = new ConnectionToSql();
        public string userid;
        //cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {   
            frmMainMenu fr = new frmMainMenu();
            fr.nameAndSurname = txtUN3 + " " + txtUN4;
            fr.userid = userid;
            fr.Show();
            this.Close();
        }

        string UN, UN2;
        // we assign username when we open this page
        string usernameTemporary;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //here, we will use different username ckeck operation. As usual we pull all usernames in a list
            byte[] name3 = ASCIIEncoding.ASCII.GetBytes(txtUN.Text);
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

            //then we look inside of that list if there is any same username in database but here is a little bit triky. what is i keep my username and just want to change other information. my username already recorded in database. so i have changed a little bit. now i am checking both is it in database and is it same which is the one when we brought back once open this page.
            foreach (var item in userNameCollection)
            {
                if (named3 == item && txtUN.Text != usernameTemporary)
                {
                    sameUserNameReader++;
                }
            }
            //rest of them is same
            if (txtUN.Text == "" || txtUN2.Text == "" || txtUN3.Text == "" || txtUN4.Text == "")
            {
                MessageBox.Show("Please provide all informations", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (sameUserNameReader > 0)
            {
                MessageBox.Show("This username has already been taken bu another user.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
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

                MessageBox.Show("User has been Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUN.Text = "";
                txtUN2.Text = "";
                txtUN3.Text = "";
                txtUN4.Text = "";
            }




        }

       
        //pass protect cancel
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            txtUN2.UseSystemPasswordChar = false;
        }
        //pass protect acivate
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            txtUN2.UseSystemPasswordChar = true;
        }

        //bring back all user infos
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
            usernameTemporary = original;
            

        }





    }
}
