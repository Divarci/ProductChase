using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductChase
{
    internal class ConnectionToSql
    {

        public SqlConnection conn()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=HASAN\SQLEXPRESS;Initial Catalog=ProductChase;Integrated Security=True");
            connection.Open();
            return connection;
        }

    }
}
