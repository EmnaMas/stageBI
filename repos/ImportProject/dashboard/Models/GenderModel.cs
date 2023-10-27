using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace dashboard.Models
{
    public class GenderModel
    {
        private static string connectionString = "Data Source=DESKTOP-1UDUIV0;Initial Catalog=DataWareHouse;Integrated Security=True;";

        public class GenderData
        {
            public string Gender { get; set; }
            public int EmailCount { get; set; }
        }

        public bool IsDatabaseConnected()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
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
                        // Ajouter le délai d'exécution maximal de 60 secondes
                        command.CommandTimeout = 180;
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GenderData genderData = new GenderData
                                {
                                    Gender = reader["Gender"].ToString(), // Utiliser le nom de colonne correct (Sexe)
                                    EmailCount = Convert.ToInt32(reader["EmailCount"])
                                };

                                genderList.Add(genderData);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Lancer une exception personnalisée pour informer l'utilisateur
                    throw new Exception("Une erreur est survenue lors de la récupération des données : " + ex.Message);
                }
            }

            return genderList;
        }
    }
}
