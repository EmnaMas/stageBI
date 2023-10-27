namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
  

    public class DimTimeOfEmailOpenedModel
    {
        public int ID { get; set; }
        public string Time { get; set; }

        public static List<DimTimeOfEmailOpenedModel> GetTimeOfEmailOpenedData()
        {
            List<DimTimeOfEmailOpenedModel> timeOfEmailOpenedData = new List<DimTimeOfEmailOpenedModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetTimeOfEmailOpenedData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DimTimeOfEmailOpenedModel timeOfEmailOpened = new DimTimeOfEmailOpenedModel();

                            timeOfEmailOpened.ID = (int)reader["ID"];
                            timeOfEmailOpened.Time = (string)reader["Time"];

                            timeOfEmailOpenedData.Add(timeOfEmailOpened);
                        }
                    }
                }
            }

            return timeOfEmailOpenedData;
        }
    }


}
