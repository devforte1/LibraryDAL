using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryDAL
{
    public class RegistrantDAL
    {
        private string _connectString;
        private LoggerDAL _logger = new LoggerDAL();

        public RegistrantDAL()
        {
            _connectString = "Data Source=localhost;Initial Catalog=LibraryMvcAppDb;Integrated Security=True";
        }

        public bool InsertUser(string[] registrant)
        {
            int result = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectString))
                {
                    SqlCommand command = new SqlCommand("dbo.sp_InsertUser", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = registrant[0];
                    command.Parameters.AddWithValue("@Password", SqlDbType.NVarChar).Value = registrant[1];
                    command.Parameters.AddWithValue("@IsAdmin", SqlDbType.NVarChar).Value = false;

                    conn.Open();
                    result = command.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string customMsg = "SqlServerDb::InsertUser()::An exception occurred accessing the stored procedure dbo.sp_InsertUser.";
                _logger.LogException(ex, customMsg);
            }

            if (result > 0)
            { return true; }
            else
            { return false; }
        }
    }
}
