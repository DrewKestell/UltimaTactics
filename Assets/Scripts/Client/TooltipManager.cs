using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TMP_Text text;

    public static TooltipManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Show()
    {
        container.SetActive(true);
    }

    public void Hide()
    {
        container.SetActive(false);
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }
}
