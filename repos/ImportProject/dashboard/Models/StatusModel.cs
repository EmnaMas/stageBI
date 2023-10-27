namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class StatusModel
    {
        public int Status { get; set; }
        public string StatusDesc { get; set; }

        public static List<StatusModel> GetStatusData()
        {
            List<StatusModel> statuses = new List<StatusModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetStatusData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StatusModel status = new StatusModel();
                            status.Status = (int)reader["Status"];
                            status.StatusDesc = (string)reader["StatusDesc"];

                            statuses.Add(status);
                        }
                    }
                }
            }

            return statuses;
        }
    }

}
