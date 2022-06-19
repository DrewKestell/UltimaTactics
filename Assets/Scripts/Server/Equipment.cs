#if SERVER_BUILD || UNITY_EDITOR
using Newtonsoft.Json;
using Unity.Netcode;

public partial class Equipment : WorldSaved
{
#if SERVER_BUILD
private int characterId = -1;

    public override void Serialize()
    {
        if (characterId == -1)
        {
            characterId = GetComponentInParent<PlayerState>().CharacterId.Value;
        }

        // Unity JsonUtility does not support dictionaries, so use Newtonsoft for now

        var equipment = GetNewtonsoftSerializable();
        var equipmentJson = JsonConvert.SerializeObject(equipment);
        SqlRepository.Instance.UpdateCharacterEquipment(equipmentJson, characterId);
    }

    // TODO: figure out a better way to get CharacterID from the GameObject hierarchy - it's tricky
    // because we need to instantiate, reparent, then spawn, so it moves in the hierarchy and causes problems on the client.
    public override void Deserialize(int characterId)
    {
        // Unity JsonUtility does not support dictionaries, so use Newtonsoft for now
        var equipmentJson = SqlRepository.Instance.GetCharacterEquipment(characterId);
        var equipment = JsonConvert.DeserializeObject<NewtonsoftSerializableEquipment>(equipmentJson);
        RightHand.Value = equipment.RightHand;
        LeftHand.Value = equipment.LeftHand;
        Head.Value = equipment.Head;
        Arms.Value = equipment.Arms;
        Neck.Value = equipment.Neck;
        Hands.Value = equipment.Hands;
        Chest.Value = equipment.Chest;
        Legs.Value = equipment.Legs;
        Feet.Value = equipment.Feet;
        Back.Value = equipment.Back;
        Waist.Value = equipment.Waist;
    }

    public object GetNewtonsoftSerializable()
    {
        return new
        {
            RightHand = RightHand.Value,
            LeftHand = LeftHand.Value,
            Head = Head.Value,
            Arms = Arms.Value,
            Neck = Neck.Value,
            Hands = Hands.Value,
            Chest = Chest.Value,
            Legs = Legs.Value,
            Feet = Feet.Value,
            Back = Back.Value,
            Waist = Waist.Value
        };
    }
#endif
}
#endif

public class NewtonsoftSerializableEquipment
{
    public SerializableItem RightHand;
    public SerializableItem LeftHand;
    public SerializableItem Head;
    public SerializableItem Arms;
    public SerializableItem Neck;
    public SerializableItem Hands;
    public SerializableItem Chest;
    public SerializableItem Legs;
    public SerializableItem Feet;
    public SerializableItem Back;
    public SerializableItem Waist;
}