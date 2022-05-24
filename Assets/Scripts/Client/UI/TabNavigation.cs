using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabNavigation : MonoBehaviour
{
    private EventSystem system;

    void Start()
    {
        system = EventSystem.current;
    }

    public void OnTab()
    {
        Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

        if (next != null)
        {
            InputField inputfield = next.GetComponent<InputField>();
            if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

            system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
        }
        // TODO: this isn't working right
        //else
        //{
        //    while (next != null)
        //    {
        //        next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();

        //        InputField inputfield = next.GetComponent<InputField>();
        //        if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

        //        system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
        //    }
        //}
    }
}
