using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    public string Text { get; set; }

    public void OnPointerEnter(BaseEventData eventData)
    {
        Debug.Log(transform.position);
        Debug.Log(GetComponent<RectTransform>().anchoredPosition);
        TooltipManager.Instance.SetText(Text);
        TooltipManager.Instance.Show();
    }

    public void OnPointerExit(BaseEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }
}
