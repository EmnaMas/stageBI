namespace dashboard.Models
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    public class EmailModel
    {
        public int EmailTrendFactID { get; set; }
        public int CtcID { get; set; }
        public int OpCode { get; set; }
        public string PrjMkgTitle { get; set; }
        public string Subject { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime OpenDate { get; set; }
        public int TimeOfDayEmailOpened { get; set; }
        public int DayOfWeekEmailOpened { get; set; }
        public string SubmittedBy { get; set; }
        public int SenderProfileID { get; set; }
        public string SenderProfile { get; set; }
        public string BrandName { get; set; }
        public string DomainName { get; set; }
        public bool IsFollowUp { get; set; }
        public bool IsComplaint { get; set; }
        public int Status { get; set; }
        public int Error { get; set; }
        public DateTime ModifiedDate { get; set; }

        public static List<EmailModel> GetEmailData()
        {
            List<EmailModel> emails = new List<EmailModel>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("GetEmailData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmailModel email = new EmailModel();

                            email.EmailTrendFactID = (int)reader["EmailTrendFactID"];
                            email.CtcID = (int)reader["CtcID"];
                            email.OpCode = (int)reader["OpCode"];
                            email.PrjMkgTitle = (string)reader["PrjMkgTitle"];
                            email.Subject = (string)reader["Subject"];
                            email.CategoryID = (int)reader["CategoryID"];
                            email.Category = (string)reader["Category"];
                            email.SendDate = (DateTime)reader["SendDate"];
                            email.OpenDate = (DateTime)reader["OpenDate"];
                            email.TimeOfDayEmailOpened = (int)reader["TimeOfDayEmailOpened"];
                            email.DayOfWeekEmailOpened = (int)reader["DayOfWeekEmailOpened"];
                            email.SubmittedBy = (string)reader["SubmittedBy"];
                            email.SenderProfileID = (int)reader["SenderProfileID"];
                            email.SenderProfile = (string)reader["SenderProfile"];
                            email.BrandName = (string)reader["BrandName"];
                            email.DomainName = (string)reader["DomainName"];
                            email.IsFollowUp = (bool)reader["IsFollowUp"];
                            email.IsComplaint = (bool)reader["IsComplaint"];
                            email.Status = (int)reader["Status"];
                            email.Error = (int)reader["Error"];
                            email.ModifiedDate = (DateTime)reader["ModifiedDate"];

                            emails.Add(email);
                        }
                    }
                }
            }

            return emails;
        }
    }

}
