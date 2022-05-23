#if SERVER_BUILD || UNITY_EDITOR
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

    public bool Authenticate(string email, string password)
    {
        var account = SqlRepository.Instance.GetAccountByEmail(email);

        if (account == null)
        {
            return false;
        }

        return HashPassword(password) == account.HashedPassword;
    }

    public string HashPassword(string password)
    {
        // TODO: implement proper hashing
        return password.GetHashCode().ToString();
    }
#endif
}
#endif