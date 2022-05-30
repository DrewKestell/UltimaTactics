using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SerializationTester
{
    [MenuItem("Serialize/Test %&c")]
    public static void SerializeTest()
    {
        var itemBase = AssetDatabase.LoadAssetAtPath("Assets/Data/Items/Mace.asset", typeof(ItemScriptableObject));
        var itemData = new ItemData
        {
            ItemBase = itemBase as ItemScriptableObject,
            Modifiers = new ItemModifierValue[]
            {
                new ItemModifierValue { Modifier = ItemModifier.ColdResist, Value = 5.0f },
                new ItemModifierValue { Modifier = ItemModifier.DamageIncrease, Value = 13.0f },
            }
        };

        var json = JsonUtility.ToJson(itemData);

        Debug.Log(json);

        var deserializedJson = JsonUtility.FromJson<ItemData>(json);

        Debug.Log(deserializedJson);
    }
}
