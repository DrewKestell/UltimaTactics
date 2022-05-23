public class Account
{
    public Account(int id, string email, string hashedPassword)
    {
        Id = id;
        Email = email;
        HashedPassword = hashedPassword;
    }

    public int Id { get; }

    public string Email { get; }

    public string HashedPassword { get; }
}
