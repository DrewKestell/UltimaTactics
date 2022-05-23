#if SERVER_BUILD || UNITY_EDITOR
using UnityEngine;
using Mono.Data.Sqlite;
using System.Collections.Generic;

public class SqlRepository : MonoBehaviour
{
#if SERVER_BUILD
    const string connectionString = @"URI=file:C:\Repos\UltimaTactics\Assets\Data\data.db"; // TODO: make this a relative path
    public static SqlRepository Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private SqliteCommand GetCommand()
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();

        return new SqliteCommand(connection);
    }

    public void InsertAccount(Account account)
    {
        var query = $"INSERT INTO Accounts (Email, HashedPassword) VALUES ('{account.Email}', '{account.HashedPassword}');";
        var command = GetCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
    }

    public void InsertCharacter(Character character)
    {
        var query = $"INSERT INTO Characters (Name, AccountId) VALUES ('{character.Name}', {character.Id});";
        var command = GetCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
    }

    public IEnumerable<Character> ListAccountCharacters(int accountId)
    {
        var query = $"SELECT * FROM Characters where AccountId = {accountId};";
        var command = GetCommand();
        command.CommandText = query;
        var reader = command.ExecuteReader();

        var characters = new List<Character>();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);

            characters.Add(new Character(id, name, accountId));
        }

        return characters;
    }

    public IEnumerable<Skill> ListSkills()
    {
        var query = "SELECT * FROM Skills;";
        var command = GetCommand();
        command.CommandText = query;
        var reader = command.ExecuteReader();

        var skills = new List<Skill>();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);

            skills.Add(new Skill(id, name));
        }

        return skills;
    }

    public Account GetAccountByEmail(string email)
    {
        var query = $"SELECT * FROM Accounts WHERE Email = {email};";
        var command = GetCommand();
        command.CommandText = query;
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var hashedPassword = reader.GetString(2);

            return new Account(id, email, hashedPassword);
        }

        return null;
    }
#endif
}
#endif