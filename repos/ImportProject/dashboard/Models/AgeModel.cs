

namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    public class AgeModel
    {
        public int AgeID { get; set; }
        public string AgeDesc { get; set; }

        public static List<AgeModel> GetAgeData()
        {
            List<AgeModel> ages = new List<AgeModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetAgeData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AgeModel age = new AgeModel();

                            age.AgeID = (int)reader["AgeID"];
                            age.AgeDesc = (string)reader["AgeDesc"];

                            ages.Add(age);
                        }
                    }
                }
            }

            return ages;
        }
    }

}
