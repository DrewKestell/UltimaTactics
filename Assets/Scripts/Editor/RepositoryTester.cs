using Mono.Data.Sqlite;
using UnityEditor;

public class RepositoryTester
{
    const string connectionString = @"Data Source=C:\Users\Drew\Repos\UltimaTactics\Assets\Data\data.db;Version=3;New=False;Compress=True;"; // TODO: make this a relative path

    [MenuItem("Repository/Test Connection %&h")]
    public static void TestConnection()
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();
    }
}
