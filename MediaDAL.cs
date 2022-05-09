using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryDAL
{
    public class MediaDAL
    {
        private string _connectString;
        private LoggerDAL _logger = new LoggerDAL();

        public MediaDAL()
        {
            _connectString = "Data Source=localhost;Initial Catalog=LibraryMvcAppDb;Integrated Security=True";
        }

        public List<string[]> SelectMediaInventory()
        {
            string mediaVal = "";
            string[] media;
            List<string[]> mediaList = new List<string[]>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectString))
                {
                    SqlCommand command = new SqlCommand("dbo.sp_SelectMediaInventory", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int mediaId = (int)reader[0];
                        int quantity = (int)reader[1];
                        string mediaType = (string)reader[2];
                        string mediaName = (string)reader[3];

                        mediaVal = ($"{reader[0]},{reader[1]},{reader[2]},{reader[3]}");
                        media = mediaVal.Split(',');
                        mediaList.Add(media);
                    }

                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string customMsg = "MediaDAL::SelectMediaInventory()::An exception occurred accessing the stored procedure dbo.sp_SelectMediaInventory.";
                _logger.LogException(ex, customMsg);
            }

            return mediaList;
        }
    }
}
