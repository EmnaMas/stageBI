using System.Data;

namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    public class DimCategoryModel
    {
        public int CategoryID { get; set; }
        public string Category { get; set; }

        public static List<DimCategoryModel> GetCategoryData()
        {
            List<DimCategoryModel> categories = new List<DimCategoryModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetCategoryData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DimCategoryModel category = new DimCategoryModel();

                            category.CategoryID = (int)reader["CategoryID"];
                            category.Category = (string)reader["Category"];

                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }

    }


}
