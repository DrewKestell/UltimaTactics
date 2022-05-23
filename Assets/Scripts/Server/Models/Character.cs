public class Character
{
    public Character(int id, string name, int accountId)
    {
        Id = id;
        Name = name;
        AccountId = accountId;
    }

    public int Id { get; }

    public string Name { get; }

    public int AccountId { get; }
}
