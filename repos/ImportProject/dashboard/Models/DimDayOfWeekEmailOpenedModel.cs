using System.Data;

namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    public class DimDayOfWeekEmailOpenedModel
    {
        public int ID { get; set; }
        public string DAY { get; set; }

        public static List<DimDayOfWeekEmailOpenedModel> GetDayOfWeekEmailOpenedData()
        {
            List<DimDayOfWeekEmailOpenedModel> dayOfWeekData = new List<DimDayOfWeekEmailOpenedModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetDayOfWeekEmailOpenedData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DimDayOfWeekEmailOpenedModel dayOfWeek = new DimDayOfWeekEmailOpenedModel();

                            dayOfWeek.ID = (int)reader["ID"];
                            dayOfWeek.DAY = (string)reader["DAY"];

                            dayOfWeekData.Add(dayOfWeek);
                        }
                    }
                }
            }

            return dayOfWeekData;
        }
    }

}
