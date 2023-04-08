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
    }
}
