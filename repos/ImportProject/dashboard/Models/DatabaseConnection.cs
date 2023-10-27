namespace dashboard.Models;
using System.Data.SqlClient;


public static class DatabaseConnection
{
    private static string connectionString = "Data Source=DESKTOP-1UDUIV0;Initial Catalog=DataWareHouse;Integrated Security=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
 


}
