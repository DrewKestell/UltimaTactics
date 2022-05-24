#if SERVER_BUILD || UNITY_EDITOR
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
#if SERVER_BUILD
    public static AccountManager Instance;

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

    public (bool, int?) Authenticate(string email, string password)
    {
        var account = SqlRepository.Instance.GetAccountByEmail(email);

        if (account == null)
        {
            return (false, null);
        }

        return (HashPassword(password) == account.HashedPassword, account.Id);
    }

    public string HashPassword(string password)
    {
        var data = Encoding.ASCII.GetBytes(password);
        using var sha1 = new SHA1CryptoServiceProvider();
        var sha1Data = sha1.ComputeHash(data);

        return Encoding.ASCII.GetString(sha1Data);
    }
#endif
}
#endif