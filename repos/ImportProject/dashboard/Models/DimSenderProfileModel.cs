namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
  
    public class DimSenderProfileModel
    {
        public int SenderProfileID { get; set; }
        public string SenderProfile { get; set; }
        public static List<DimSenderProfileModel> GetSenderProfileData()
        {
            List<DimSenderProfileModel> senderProfileData = new List<DimSenderProfileModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetSenderProfileData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DimSenderProfileModel senderProfile = new DimSenderProfileModel();

                            senderProfile.SenderProfileID = (int)reader["SenderProfileID"];
                            senderProfile.SenderProfile = (string)reader["SenderProfile"];

                            senderProfileData.Add(senderProfile);
                        }
                    }
                }
            }

            return senderProfileData;
        }
    }


}
