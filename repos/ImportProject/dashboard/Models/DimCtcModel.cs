using System.Data;

namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    public class DimCtcModel
    {
        public int CtcID { get; set; }
        public int AgeID { get; set; }
        public int Gender { get; set; }

        public static List<DimCtcModel> GetCtcData()
        {
            List<DimCtcModel> ctcData = new List<DimCtcModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetCtcData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DimCtcModel ctc = new DimCtcModel();

                            ctc.CtcID = (int)reader["CtcID"];
                            ctc.AgeID = (int)reader["AgeID"];
                            ctc.Gender = (int)reader["Gender"];

                            ctcData.Add(ctc);
                        }
                    }
                }
            }

            return ctcData;
        }
    }

}
