using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class SerializationTester
{
    [MenuItem("Serialize/Test %&c")]
    public static void SerializeTest()
    {
        //var so = AssetDatabase.LoadAssetAtPath("Assets/Data/Items/Mace.asset", typeof(ItemScriptableObject));
        //var data = new ItemData
        //{
        //    ItemBase = so as ItemScriptableObject,
        //    Modifiers = new ItemModifierValue[]
        //    {
        //        new ItemModifierValue { Modifier = ItemModifier.ColdResist, Value = 5.0f },
        //        new ItemModifierValue { Modifier = ItemModifier.DamageIncrease, Value = 13.0f },
        //    }
        //};

        //var so1 = AssetDatabase.LoadAssetAtPath("Assets/Data/Skills/Anatomy.asset", typeof(SkillScriptableObject));
        //var so2 = AssetDatabase.LoadAssetAtPath("Assets/Data/Skills/Alchemy.asset", typeof(SkillScriptableObject));
        //var so3 = AssetDatabase.LoadAssetAtPath("Assets/Data/Skills/Archery.asset", typeof(SkillScriptableObject));
        //var data = new SkillsData
        //{
        //    SkillValues = new SkillScriptableObject[]
        //    {
        //        so1 as SkillScriptableObject,
        //        so2 as SkillScriptableObject,
        //        so3 as SkillScriptableObject
        //    }
        //};
        //var json = JsonUtility.ToJson(data);

        //Debug.Log(json);

        //var deserializedJson = JsonUtility.FromJson<SkillsData>(json);

        //Debug.Log(deserializedJson);

        //var foo = new float[]
        //{
        //    1, 3, 5
        //};

        //var json = JsonUtility.ToJson(foo);

        //Debug.Log(json);

        //var deserializedJson = JsonUtility.FromJson<float[]>(json);

        //Debug.Log(deserializedJson.Length);

        // Copy Mesh and Material from imported model

        //var player = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Player.prefab");
        //var belt = player.transform.GetChild(0);
        //var smr = belt.GetComponent<SkinnedMeshRenderer>();

        //var newMesh = Object.Instantiate(smr.sharedMesh);
        //AssetDatabase.CreateAsset(newMesh, "Assets/Meshes/Belt.asset");

        //var newMaterial = Object.Instantiate(smr.sharedMaterial);
        //AssetDatabase.CreateAsset(newMaterial, "Assets/Materials/Belt.mat");

        //AssetDatabase.SaveAssets();

        // Test object creation
        var player = GameObject.Find("Player");
        var equipmentTemplate = player.transform.GetChild(0);
        var equipmentObj = Object.Instantiate(equipmentTemplate, player.transform);
        equipmentObj.name = "Belt";
        var smr = equipmentObj.GetComponent<SkinnedMeshRenderer>();

        var so1 = AssetDatabase.LoadAssetAtPath("Assets/Data/Items/Belt.asset", typeof(ItemScriptableObject)) as ItemScriptableObject;
        smr.sharedMesh = so1.Mesh;
        smr.sharedMaterial = so1.Material;
    }
}
