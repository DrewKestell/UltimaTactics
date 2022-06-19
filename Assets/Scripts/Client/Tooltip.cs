using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private ClickTarget clickTarget;

    public string Text { get; set; }

    public void OnPointerEnter(BaseEventData eventData)
    {
        TooltipManager.Instance.SetText(transform.position, Text);
        TooltipManager.Instance.Show();
    }

    public void OnPointerExit(BaseEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        var ped = eventData as PointerEventData;
        if (ped.button == PointerEventData.InputButton.Right)
        {
            ContextMenu.Instance.Activate(clickTarget, ped.position);
        }
    }
}
