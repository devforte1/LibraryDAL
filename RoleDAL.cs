using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryDAL
{
    public class RoleDAL
    {
        private string _connectString;
        private LoggerDAL _logger = new LoggerDAL();

        public RoleDAL()
        {
            _connectString = "Data Source=localhost;Initial Catalog=LibraryMvcAppDb;Integrated Security=True";
        }

        public List<string[]> SelectRoles()
        {
            string roleVal = "";
            string[] role;
            List<string[]> roleList = new List<string[]>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectString))
                {
                    SqlCommand command = new SqlCommand("dbo.sp_SelectRoles", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]},{reader[1]}");

                        int roleId = (int)reader[0];
                        string roleName = (string)reader[1];

                        roleVal = ($"{reader[0]},{reader[1]}");
                        role = roleVal.Split(',');
                        roleList.Add(role);
                    }

                    reader.Close();
                    conn.Close();
                }


            }
            catch (Exception ex)
            {
                string customMsg = "RoleDAL::GetRoles()::An exception occurred accessing the stored procedure dbo.sp_SelectRoles.";
                _logger.LogException(ex, customMsg);
            }

            return roleList;
        }
    }
}
