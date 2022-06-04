#if SERVER_BUILD || UNITY_EDITOR
using UnityEngine;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using Newtonsoft.Json;

public class SqlRepository : MonoBehaviour
{
#if SERVER_BUILD
    const string connectionString = @"Data Source=C:\Repos\UltimaTactics\Assets\Data\data.db;Version=3;New=False;Foreign Key Constraints=On;Compress=True;"; // TODO: make this a relative path
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
    
    private int GetLastId(SqliteCommand command)
    {
        command.CommandText = "select last_insert_rowid()";
        var lastRowId64 = (long)command.ExecuteScalar();

        return (int)lastRowId64;
    }

    public int InsertAccount(Account account)
    {
        var query = $"INSERT INTO Accounts (Email, HashedPassword) VALUES ('{account.Email}', '{account.HashedPassword}')";
        var command = GetCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();

        return GetLastId(command);
    }

    public int InsertCharacter(Character character)
    {
        var query = $"INSERT INTO Characters (Name, AccountId) VALUES ('{character.Name}', {character.AccountId})";
        var command = GetCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();

        return GetLastId(command);
    }

    public IEnumerable<Character> ListAccountCharacters(string email)
    {
        var account = GetAccountByEmail(email);
        if (account != null)
        {
            var query = $"SELECT * FROM Characters where AccountId = {account.Id}";
            var command = GetCommand();
            command.CommandText = query;
            var reader = command.ExecuteReader();

            var characters = new List<Character>();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);

                characters.Add(new Character(id, name, account.Id));
            }

            return characters;
        }

        return null;
    }

    public Account GetAccountByEmail(string email)
    {
        var query = $"SELECT * FROM Accounts WHERE Email = '{email}'";
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

    public void InsertCharacterSkills(string skillsJson, int characterId)
    {
        var query = $"INSERT INTO CharacterSkills (Skills, CharacterId) VALUES ('{skillsJson}', {characterId})";
        var command = GetCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
    }

    public void UpdateCharacterSkills(string skillsJson, int characterId)
    {
        var query = $"UPDATE CharacterSkills SET Skills = '{skillsJson}' WHERE CharacterId = {characterId}";
        var command = GetCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();
    }

    public Character GetCharacter(int characterId)
    {
        var query = $"SELECT * FROM Characters WHERE Id = {characterId}";
        var command = GetCommand();
        command.CommandText = query;
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var name = reader.GetString(1);
            var accountId = reader.GetInt32(2);

            return new Character(characterId, name, accountId);
        }

        return null;
    }

    public string GetCharacterSkills(int characterId)
    {
        var query = $"SELECT * FROM CharacterSkills WHERE CharacterId = {characterId}";
        var command = GetCommand();
        command.CommandText = query;
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            return reader.GetString(1);
        }

        return null;
    }
#endif
}
#endif