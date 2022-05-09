using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryDAL
{
    public class UserDAL
    {
        private string _connectString;
        private LoggerDAL _logger = new LoggerDAL();

        public UserDAL()
        {
            _connectString = "Data Source=localhost;Initial Catalog=LibraryMvcAppDb;Integrated Security=True";
        }

        public List<string[]> SelectUsers()
        {
            string userVal = "";
            string[] user;
            List<string[]> userList = new List<string[]>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectString))
                {
                    SqlCommand command = new SqlCommand("dbo.sp_SelectUsers", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]},{reader[1]},{reader[2]},{reader[3]}");

                        int userId = (int)reader[0];
                        string userName = (string)reader[1];
                        string userPassword = (string)reader[2];
                        bool isAdmin = (bool)reader[3];

                        userVal = ($"{reader[0]},{reader[1]},{reader[2]},{reader[3]}");
                        user = userVal.Split(',');
                        userList.Add(user);
                    }

                    reader.Close();
                    conn.Close();
                }


            }
            catch (Exception ex)
            {
                string customMsg = "UserDAL::GetUsers()::An exception occurred accessing the stored procedure dbo.sp_SelectUsers.";
                _logger.LogException(ex, customMsg);
            }

            return userList;
        }

        public bool AuthenticateUser(string userName, string password)
        {
            UserDAL userDAL = new UserDAL();

            try
            {
                List<string[]> users = userDAL.SelectUsers();
                foreach (string[] user in users)
                {
                    if (user[1].ToString() == userName && user[2].ToString() == password)
                    {
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                string customMsg = "UserDAL::AuthenticateUser()::An exception occurred accessing the stored procedure dbo.sp_AuthenticateUser.";
                _logger.LogException(ex, customMsg);
            }

            return false;
        }

        public string[] SelectUserByUserName(string userName)
        {
            string userVal = "";
            string[] user = new string[1];

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectString))
                {
                    SqlCommand command = new SqlCommand("dbo.sp_SelectUserByUserName", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", SqlDbType.NVarChar).Value = userName;
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int userId = (int)reader[0];
                        string name = (string)reader[1];
                        string userPassword = (string)reader[2];
                        bool isAdmin = (bool)reader[3];

                        userVal = ($"{reader[0]},{reader[1]},{reader[2]},{reader[3]}");
                        user = userVal.Split(',');
                    }

                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string customMsg = "SqlServerDb::SelectUserByName()::An exception occurred accessing the stored procedure dbo.sp_SelectUserByUserName.";
                _logger.LogException(ex, customMsg);
            }

            return user;
        }
    }
}
