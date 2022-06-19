using Unity.Netcode;

public partial class Equipment : WorldSaved
{
    public NetworkVariable<SerializableItem> RightHand;
    public NetworkVariable<SerializableItem> LeftHand;
    public NetworkVariable<SerializableItem> Head;
    public NetworkVariable<SerializableItem> Arms;
    public NetworkVariable<SerializableItem> Neck;
    public NetworkVariable<SerializableItem> Hands;
    public NetworkVariable<SerializableItem> Chest;
    public NetworkVariable<SerializableItem> Legs;
    public NetworkVariable<SerializableItem> Feet;
    public NetworkVariable<SerializableItem> Back;
    public NetworkVariable<SerializableItem> Waist;

    private void Awake()
    {
        RightHand = new();
        LeftHand = new();
        Head = new();
        Arms = new();
        Neck = new();
        Hands = new();
        Chest = new();
        Legs = new();
        Feet = new();
        Back = new();
        Waist = new();
    }
}
