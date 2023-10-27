using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace DashboardBI.Models
{


    public class GenderModel
    {
        private static string connectionString = "Data Source=DESKTOP-1UDUIV0;Initial Catalog=DataWareHouse;Integrated Security=True;";

        public class GenderData
        {
            public string Sexe { get; set; }
            public int EmailCount { get; set; }
        }

        public List<GenderData> GetEmailsByGender()
        {
            List<GenderData> genderList = new List<GenderData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetEmailsByGender", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GenderData genderData = new GenderData
                                {
                                    Sexe = reader["sexe"].ToString(),
                                    EmailCount = Convert.ToInt32(reader["EmailCount"])
                                };

                                genderList.Add(genderData);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Gérer les erreurs ici si nécessaire
                    Console.WriteLine(ex.Message);
                }
            }

            return genderList;
        }
    }

}
