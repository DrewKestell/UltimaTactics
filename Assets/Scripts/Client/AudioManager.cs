#if CLIENT_BUILD || UNITY_EDITOR
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip vesperMusic;

    private AudioSource audioSource;

#if CLIENT_BUILD
    public static AudioManager Instance;

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

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = vesperMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
#endif
}
#endif
