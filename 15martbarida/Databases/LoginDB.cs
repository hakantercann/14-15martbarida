using _15martbarida.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15martbarida.Databases
{
    public class LoginDB
    {
        public bool Login(string username, string userpass)
        {
            string userName = string.Empty;
            string roleName = string.Empty;
            string result = string.Empty;
            SqlConnection conn = Database.GetSqlConnection();
            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = "loginProcess";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@username", System.Data.SqlDbType.NVarChar)).Value = username;
                    command.Parameters.Add(new SqlParameter("@userpass", System.Data.SqlDbType.NVarChar)).Value = userpass;
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        result = reader["result"].ToString();
                        userName = reader["username"].ToString();
                        roleName = reader["rolename"].ToString();

                    }
                    
                    command.Dispose();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (result.Equals("1"))
                    {
                        conn.Close();

                        TemporaryMemory.IsLoginned = false ;
                        return false;
                    }
                    else
                    {
                        TemporaryMemory.name = userName;
                        int temp = Convert.ToInt32(Enum.Parse(typeof(Roles), roleName));
                        TemporaryMemory.Role = (Roles)temp;
                        TemporaryMemory.IsLoginned = true;
                        Session.startSession();
                        command.Dispose();
                        conn.Close();
                        return true;
                    }

                }
            }
            catch
            {
                return false;
            }
            
        }

    }
}
