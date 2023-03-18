using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15martbarida.Databases
{
    public static class Database
    {
        public static SqlConnection GetSqlConnection()
        {
            string cn_String = Properties.Settings.Default.onbesmartdutyConnectionString;

            SqlConnection cn_connection = new SqlConnection(cn_String);

            try
            {

                if (cn_connection.State != ConnectionState.Open) cn_connection.Open();
            }
            catch
            { return null; }
            //</ db oeffnen >



            //< output >

            return cn_connection;
        }
    }
}
