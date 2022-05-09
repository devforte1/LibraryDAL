using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL
{
    public class LoggerDAL
    {
        private string _connectString;

        public LoggerDAL()
        {
            _connectString = "Data Source=localhost;Initial Catalog=LibraryMvcAppDb;Integrated Security=True";
        }

        public bool LogException(Exception ex, string customMsg = "N/A")
        {
            int result = 0;
 
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectString))
                {
                    SqlCommand command = new SqlCommand("dbo.sp_InsertException", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StackTrace", SqlDbType.NVarChar).Value = ex.StackTrace.ToString();
                    command.Parameters.AddWithValue("@SystemMsg", SqlDbType.NVarChar).Value = ex.Message.ToString();
                    command.Parameters.AddWithValue("@CustomMsg", SqlDbType.NVarChar).Value = customMsg;
                    command.Parameters.AddWithValue("@ExceptionSource", SqlDbType.NVarChar).Value = ex.Source.ToString();
                    command.Parameters.AddWithValue("@ExceptionType", SqlDbType.NVarChar).Value = ex.GetType().ToString();
                    command.Parameters.AddWithValue("@URL", SqlDbType.NVarChar).Value = "FindUrl";

                    conn.Open();
                    result = command.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex2)
            {
                string customMsg2 = "LoggerDAL::LogException()::An exception occurred accessing the stored procedure dbo.sp_InsertException.";
                Console.WriteLine(customMsg2);
                Console.WriteLine(ex2.Message);
            }

            if (result > 0)
            { return true; }
            else
            { return false; }

        }
    }
}
