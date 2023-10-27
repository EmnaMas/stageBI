namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
 
    public class DimErrorModel
    {
        public int Error { get; set; }
        public string ErrorDescription { get; set; }

        public static List<DimErrorModel> GetErrorData()
        {
            List<DimErrorModel> errorData = new List<DimErrorModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetErrorData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DimErrorModel error = new DimErrorModel();
                            error.Error = (int)reader["Error"];
                            error.ErrorDescription = (string)reader["ErrorDescription"];

                            errorData.Add(error);
                        }
                    }
                }
            }

            return errorData;
        }
    }

}
