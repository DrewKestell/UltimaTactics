#if CLIENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private RectTransform scrollableContent;
    [SerializeField] private GameObject inventoryItemPrefab;

    private readonly Dictionary<int, GameObject> itemMap = new();

#if CLIENT_BUILD
    private void Awake()
    {
        gameObject.transform.parent.gameObject.SetActive(false);

        PubSub.Instance.Subscribe<ItemAddedToInventoryEvent>(this, HandleItemAdded);
        PubSub.Instance.Subscribe<ItemRemovedFromInventoryEvent>(this, HandleItemRemoved);
    }

    public void HandleItemAdded(ItemAddedToInventoryEvent e)
    {
        var instance = Instantiate(inventoryItemPrefab, scrollableContent);
        itemMap.Add(e.ItemId, instance);
        var iso = ItemManager.Instance.AllItems[e.Item.Name];
        instance.transform.GetChild(1).GetComponent<Image>().sprite = iso.InventorySprite;

        var text = string.Empty;
        if (iso.Type == ItemType.Armor)
        {
            var aso = iso as ArmorScriptableObject;
            var sb = new StringBuilder();
            sb.Append($@"<b>Belt</b>

<b>Type:</b> {iso.Type}
<b>Equip Slot:</b> {iso.EquipSlot}

<b>Blunt Resistance:</b> {aso.BluntResistance}
<b>Slash Resistance:</b> {aso.SlashResistance}
<b>Pierce Resistance:</b> {aso.PierceResistance}

<b>Physical Resistance:</b> {aso.PhysicalResistance}
<b>Fire Resistance:</b> {aso.FireResistance}
<b>Cold Resistance:</b> {aso.ColdResistance}
<b>Poison Resistance:</b> {aso.PoisonResistance}
<b>Energy Resistance:</b> {aso.EnergyResistance}");

            if (e.Item.Modifiers.Length > 0)
            {
                sb.AppendLine(string.Empty);
                sb.AppendLine(string.Empty);
            }

            foreach (var mod in e.Item.Modifiers)
            {
                sb.AppendLine($"<b><color=#7fff00>{mod.Modifier.GetAttribute<DisplayAttribute>().Name}:</color></b> {mod.Value}");
            }

            text = sb.ToString();
        }
        else
        {
            // TODO
            text = "TODO";
        }

        instance.transform.GetChild(2).GetComponent<Tooltip>().Text = text;
    }

    public void HandleItemRemoved(ItemRemovedFromInventoryEvent e)
    {
        
    }
#endif
}
#endif
