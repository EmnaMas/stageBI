using System.Data;

namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    public class DimCodeModel
    {
        public int Opcode { get; set; }
        public string Title { get; set; }

        public static List<DimCodeModel> GetCodeData()
        {
            List<DimCodeModel> codes = new List<DimCodeModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetCodeData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DimCodeModel code = new DimCodeModel();

                            code.Opcode = (int)reader["Opcode"];
                            code.Title = (string)reader["Title"];

                            codes.Add(code);
                        }
                    }
                }
            }

            return codes;
        }
    }

}
