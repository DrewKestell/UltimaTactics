using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ArmorScriptableObject", order = 1)]
public class ArmorScriptableObject : ItemScriptableObject
{
    public int BluntResistance;
    public int SlashResistance;
    public int PierceResistance;

    public int PhysicalResistance;
    public int FireResistance;
    public int ColdResistance;
    public int PoisonResistance;
    public int EnergyResistance;
}
