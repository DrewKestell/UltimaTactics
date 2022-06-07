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

    public void SetText(Vector2 tooltipPos, string newText)
    {
        text.text = newText;
        var size = text.GetPreferredValues();
        var containerRect = container.GetComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(size.x + 20, size.y + 20);

        var currentScale = Screen.width / (float)1920;
        var containerWidth = containerRect.sizeDelta.x;
        var newWidth = ((containerWidth / 2) + 30) * currentScale;
        var newPosition = new Vector2(tooltipPos.x + newWidth, tooltipPos.y);
        container.transform.position = newPosition;
    }
}
