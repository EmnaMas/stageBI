using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ImportProject
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=DESKTOP-1UDUIV0;Initial Catalog=DataImport;Integrated Security=True;";
        private HttpClient client;

        public Form1()
        {
            InitializeComponent();
            client = new HttpClient();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string tokenApiUrl = "https://testonebrand.atreemo.com/token";
            string dataApiUrl = "https://testonebrand.atreemo.com/api/BIDWHouse/GetEmailTrendFact?PageSize=10000";
            string postApiUrl = "https://testonebrand.atreemo.com/api/BIDWHouse/Post";
            string tableName = "EmailTrendFact";

            string username = "EmnaMasmoudi";
            string password = "Masmoudiemna02@09@1999";
            string grantType = "password";

            try
            {
                List<EmailTrendFact> allEmails = new List<EmailTrendFact>(); // Liste pour stocker les données de tous les mois

                for (int month = 1; month <= 12; month++)
                {
                    DateTime startDate = new DateTime(2020, month, 1);

                    // Construire le contenu de la requête pour obtenir le jeton
                    var tokenRequestData = new Dictionary<string, string>
                    {
                        { "username", username },
                        { "password", password },
                        { "grant_type", grantType }
                    };

                    var tokenRequestContent = new FormUrlEncodedContent(tokenRequestData);

                    HttpResponseMessage tokenResponse = await client.PostAsync(tokenApiUrl, tokenRequestContent);

                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        var tokenResponseContent = await tokenResponse.Content.ReadAsStringAsync();
                        var tokenResponseObj = JsonConvert.DeserializeObject<TokenResponse>(tokenResponseContent);

                        string token = tokenResponseObj.access_token;

                        // Envoi de la date par la méthode POST
                        var postRequestData = new
                        {
                            TableName = tableName,
                            StartDate = startDate.ToString("yyyy-MM-dd")
                        };

                        var postRequestContent = new StringContent(JsonConvert.SerializeObject(postRequestData), Encoding.UTF8, "application/json");

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                        HttpResponseMessage postResponse = await client.PostAsync(postApiUrl, postRequestContent);

                        if (postResponse.IsSuccessStatusCode)
                        {
                            HttpResponseMessage dataResponse = await client.GetAsync(dataApiUrl);

                            if (dataResponse.IsSuccessStatusCode)
                            {
                                var dataResponseContent = await dataResponse.Content.ReadAsStringAsync();
                                var emailResponse = JsonConvert.DeserializeObject<EmailResponse>(dataResponseContent);
                                var listOfEmails = emailResponse.EmailTrendFacts;

                                allEmails.AddRange(listOfEmails); // Ajouter les données de chaque mois à la liste
                            }
                            else
                            {
                                MessageBox.Show($"Erreur lors de la requête API GET pour tous les mois: {dataResponse.StatusCode}");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la requête API POST : " + postResponse.StatusCode);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la requête API Token : " + tokenResponse.StatusCode);
                    }
                }

                // Appeler la fonction pour insérer les données dans la base de données
                InsertDataToDatabase(allEmails);

                MessageBox.Show("Les données ont été importées avec succès pour tous les mois");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite : " + ex.Message);
            }
        }

        void InsertDataToDatabase(List<EmailTrendFact> allEmails)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.DestinationTableName = "VersionFinal";
                        bulkCopy.WriteToServer(ConvertToDataTable(allEmails));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de l'insertion des données dans la base de données : " + ex.Message);
            }
        }

        private DataTable ConvertToDataTable(List<EmailTrendFact> emails)
        {
            DataTable dataTable = new DataTable();

            // Ajouter les colonnes correspondantes à votre table SQL
            dataTable.Columns.Add("EmailTrendFactID", typeof(int));
            dataTable.Columns.Add("CtcID", typeof(int));
            dataTable.Columns.Add("OpCode", typeof(int));
            dataTable.Columns.Add("PrjMkgTitle", typeof(string));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("CategoryID", typeof(int));
            dataTable.Columns.Add("Category", typeof(string));
            dataTable.Columns.Add("SendDate", typeof(DateTime));
            dataTable.Columns.Add("OpenDate", typeof(DateTime));
            dataTable.Columns.Add("TimeOfDayEmailOpened", typeof(int));
            dataTable.Columns.Add("DayOfWeekEmailOpened", typeof(int));
            dataTable.Columns.Add("SubmittedBy", typeof(string));
            dataTable.Columns.Add("SenderProfileID", typeof(int));
            dataTable.Columns.Add("SenderProfile", typeof(string));
            dataTable.Columns.Add("BrandName", typeof(string));
            dataTable.Columns.Add("DomainName", typeof(string));
            dataTable.Columns.Add("IsFollowUp", typeof(bool));
            dataTable.Columns.Add("IsComplaint", typeof(bool));
            dataTable.Columns.Add("Status", typeof(int));
            dataTable.Columns.Add("Error", typeof(int));
            dataTable.Columns.Add("FirstTrial", typeof(DateTime));
            dataTable.Columns.Add("LastTrial", typeof(DateTime));
            dataTable.Columns.Add("NbTrial", typeof(int));
            dataTable.Columns.Add("NbClicks", typeof(int));
            dataTable.Columns.Add("NbViews", typeof(int));
            dataTable.Columns.Add("Unsubscribe", typeof(int));
            dataTable.Columns.Add("CostPerItem", typeof(decimal));
            dataTable.Columns.Add("ModifiedDate", typeof(DateTime));

            foreach (var item in emails)
            {
                dataTable.Rows.Add(
                    item.EmailTrendFactID,
                    item.CtcID,
                    item.OpCode,
                    item.PrjMkgTitle,
                    item.Subject,
                    item.CategoryID,
                    item.Category,
                    GetNullableSqlDateTimeValue(item.SendDate),
                    GetNullableSqlDateTimeValue(item.OpenDate),
                    item.TimeOfDayEmailOpened,
                    item.DayOfWeekEmailOpened,
                    item.SubmittedBy,
                    item.SenderProfileID,
                    item.SenderProfile,
                    item.BrandName,
                    item.DomainName,
                    item.IsFollowUp,
                    item.IsComplaint,
                    item.Status,
                    item.Error,
                    GetNullableSqlDateTimeValue(item.FirstTrial),
                    GetNullableSqlDateTimeValue(item.LastTrial),
                    item.NbTrial,
                    item.NbClicks,
                    item.NbViews,
                    item.Unsubscribe,
                    item.CostPerItem,
                    GetNullableSqlDateTimeValue(item.ModifiedDate)
                );
            }

            return dataTable;
        }

        // Méthode pour vérifier si une date est valide pour SQL Server
        bool IsValidSqlDateTime(DateTime dateTime)
        {
            // Les dates valides pour SQL Server doivent être comprises entre 1/1/1753 12:00:00 AM et 31/12/9999 11:59:59 PM
            DateTime minSqlDateTime = new DateTime(1753, 1, 1);
            DateTime maxSqlDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 999);

            return (dateTime >= minSqlDateTime && dateTime <= maxSqlDateTime);
        }

        // Fonction pour obtenir la valeur de date appropriée (date ou DBNull.Value)
        object GetNullableSqlDateTimeValue(DateTime? dateTime)
        {
            return dateTime.HasValue && IsValidSqlDateTime(dateTime.Value) ? (object)dateTime.Value : DBNull.Value;
        }
    }
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    public class EmailResponse
    {
        public List<EmailTrendFact> EmailTrendFacts { get; set; }
    }

    public class EmailTrendFact
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
        public DateTime FirstTrial { get; set; }
        public DateTime LastTrial { get; set; }
        public int NbTrial { get; set; }
        public int NbClicks { get; set; }
        public int NbViews { get; set; }
        public int Unsubscribe { get; set; }
        public decimal CostPerItem { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
