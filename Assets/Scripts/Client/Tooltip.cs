using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
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
}
