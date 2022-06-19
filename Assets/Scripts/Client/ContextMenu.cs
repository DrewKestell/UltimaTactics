using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    public static ContextMenu Instance;

    [SerializeField] private GameObject contextMenuContainer;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button destroyButton;

    private ClickTarget clickTarget;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        equipButton.onClick.AddListener(OnClickEquipButton);
    }

    public void OnClickEquipButton()
    {

    }

    public void Activate(ClickTarget clickTarget, Vector2 mousePos)
    {
        this.clickTarget = clickTarget;
        TooltipManager.Instance.Hide();

        int buttonCount = 0;
        switch (clickTarget)
        {
            case ClickTarget.UnwornEquipment:
                contextMenuContainer.SetActive(true);
                destroyButton.gameObject.SetActive(true);
                equipButton.gameObject.SetActive(true);
                buttonCount = 2;
                break;
            case ClickTarget.WornEquipment:
                contextMenuContainer.SetActive(true);
                destroyButton.gameObject.SetActive(true);
                unequipButton.gameObject.SetActive(true);
                buttonCount = 2;
                break;
        }

        var containerRect = contextMenuContainer.GetComponent<RectTransform>();

        var newHeight = (buttonCount * 50) + 6 + (3 * buttonCount - 1); // 50px per button, 3px top and bottom margin, and 3px inbetween each button
        containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, newHeight);

        var currentScale = Screen.width / (float)1920;
        var posOffsetX = (containerRect.sizeDelta.x / 2) * currentScale;
        var posOffsetY = (containerRect.sizeDelta.y / 2) * currentScale;

        contextMenuContainer.transform.position = new Vector2(mousePos.x + posOffsetX, mousePos.y + posOffsetY);
    }

    public void Deactivate()
    {
        contextMenuContainer.SetActive(false);
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
        destroyButton.gameObject.SetActive(false);
    }

    public bool IsActive => contextMenuContainer.activeInHierarchy;
}
