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
    public partial class frmMainMenu : Form
    {
        public frmMainMenu()
        {
            InitializeComponent();
        }

        ConnectionToSql conn = new ConnectionToSql();

        private void btnCategories_Click(object sender, EventArgs e)
        {
            frmCategories fr = new frmCategories();
            fr.Show();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            frmEmployee fr = new frmEmployee();
            fr.Show();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            frmProducts fr = new frmProducts();
            fr.Show();
        }

        private void btnClients_Click(object sender, EventArgs e)
        {
            frmClient fr = new frmClient();
            fr.Show();
        }

        private void btnMovement_Click(object sender, EventArgs e)
        {
            frmMovement fr = new frmMovement();
            fr.Show();
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            frmStatistics fr = new frmStatistics();
            fr.Show();
        }

        private void frmMainMenu_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("BEST_CITY", conn.conn());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                crtCities.Series["Cities"].Points.AddXY(dr[0], dr[1]);
            }
            conn.conn().Close();

            SqlCommand cmd2 = new SqlCommand("BEST_EMPLOYEE", conn.conn());
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                crtEmployee.Series["Employee"].Points.AddXY(dr2[0], dr2[1]);
            }
            conn.conn().Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
