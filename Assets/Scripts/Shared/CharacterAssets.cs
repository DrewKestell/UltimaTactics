using System.Collections.Generic;
using Unity.Netcode;

public class CharacterAssets : INetworkSerializable
{
    public string Name;

    public int[] SkillIds;
    public float[] SkillValues;

    private Dictionary<int, float> skills;
    public Dictionary<int, float> Skills
    {
        get
        {
            if (skills == null)
            {
                skills = new Dictionary<int, float>();

                for (var i = 0; i < SkillIds.Length; i++)
                {
                    skills.Add(SkillIds[i], SkillValues[i]);
                }
            }

            return skills;
        }
    }

    public Inventory Inventory;
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Name);
        serializer.SerializeValue(ref SkillIds);
        serializer.SerializeValue(ref SkillValues);
        Inventory.NetworkSerialize(serializer);
    }
}
