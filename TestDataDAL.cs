using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDAL
{
    public class TestDataDAL
    {
        private string _connectString;
        private LoggerDAL _logger = new LoggerDAL();

        public TestDataDAL()
        {
            _connectString = "Data Source=localhost;Initial Catalog=LibraryMvcAppDb;Integrated Security=True";
        }

        public bool LoadTestData()
        {
            int result = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectString))
                {
                    SqlCommand command = new SqlCommand("dbo.sp_LoadTestData", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    result = command.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string customMsg = "TestDataDAL::LoadTestData()::An exception occurred accessing the stored procedure dbo.sp_LoadTestData.";
                _logger.LogException(ex, customMsg);
            }

            if (result > 0)
            { return true; }
            else
            { return false; }
        }
    }
}
